$(document).ready(function () {

    /* Datatable starts here */
    const dataTable = $('#usersTable').DataTable({
        dom: "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Add',
                className: "btn btn-success",
                attr: {
                    id: "btnAdd"
                },
                action: function ( e, dt, node, config ) {
                }
            },
            {
                text: 'Refresh',
                className: "btn btn-warning",
                action: function ( e, dt, node, config ) {
                   $.ajax({
                        type: 'GET',
                        url: '/Admin/User/GetAllUsers/',
                        contentType: 'application/json',
                        beforeSend: function(){
                            $("#spinner").show();
                            $("#usersTable").hide();
                        },
                        success: function(data){
                            var userListDto = jQuery.parseJSON(data);
                            if(userListDto.ResultStatus === 0){
                                dataTable.clear();
                                $.each(userListDto.Users.$values,function(index,user){
                                    const tableRow = dataTable.row.add([
                                        user.Id,
                                        user.UserName,
                                        user.Email,
                                        user.FirstName,
                                        user.LastName,
                                        user.PhoneNumber,
                                        user.About.length > 75 ? user.About.substring(0, 75) : user.About,
                                        `
                                            <img src="/img/${user.Picture}" alt="picture" style="max-width: 50px;">
                                        `,
                                        `
                                            <button class="btn btn-info btn-sm btn-detail" data-id="${user.Id}"><span class="fas fa-newspaper"></span></button>
                                            <button class="btn btn-warning btn-sm btn-assign" data-id="${user.Id}"><span class="fas fa-user-shield"></span></button>
                                            <button class="btn btn-primary btn-sm btn-update" data-id="${user.Id}"><span class="fas fa-edit"></span></button>
                                            <button class="btn btn-danger btn-sm btn-delete" data-id="${user.Id}"><span class="fas fa-minus-circle"></span></button>
                                        `
                                    ]).node();
                                    const jQueryTableRow = $(tableRow);
                                    jQueryTableRow.attr("data-id",`user-row-${user.Id}`)
                                });
                                dataTable.draw();
                                $("#spinner").hide();
                                $("#usersTable").fadeIn(2000);
                            }else{
                                toastr.error(`${userListDto.Message}`);
                                $("#spinner").hide();
                                $("#usersTable").fadeIn(1000);
                            }
                        },
                        error: function(err){
                            toastr.error(`${err.responseText}`);
                                $("#spinner").hide();
                                $("#usersTable").fadeIn(1000);
                        }
                   })
                }
            }
        ]
    });
    /* Datatable ends here */

    $(function () {

        /* Get request for Add method starts here */
        const modalPlaceHolder = $("#modalPlaceHolder");
        const url = '/Admin/User/Add/';
        $("#btnAdd").click(function(){
            $.get(url).done(function(data){
                modalPlaceHolder.html(data);
                modalPlaceHolder.find(".modal").modal("show");
            })
        })
        /* Get request for Add method ends here */

        /* Post request for Add method starts here */
        modalPlaceHolder.on("click","#btnSave",function(e){
            e.preventDefault();
            const form = $("#user-add-form");
            const actionUrl = form.attr("action");
            const dataToSend = new FormData(form.get(0));
            $.ajax({
                type: 'POST',
                url: actionUrl,
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function(data){
                    const userAddAjaxViewModel = jQuery.parseJSON(data);
                    const newModalBody = $(".modal-body",userAddAjaxViewModel.UserAddPartial);
                    modalPlaceHolder.find(".modal-body").replaceWith(newModalBody);
                    const IsValid = newModalBody.find("[name='IsValid']").val() === "True";
                    if(IsValid){
                        modalPlaceHolder.find(".modal").modal("hide");
                        const tableRow = dataTable.row.add([
                            userAddAjaxModel.UserDto.User.Id,
                            userAddAjaxModel.UserDto.User.UserName,
                            userAddAjaxModel.UserDto.User.Email,
                            userAddAjaxModel.UserDto.User.FirstName,
                            userAddAjaxModel.UserDto.User.LastName,
                            userAddAjaxModel.UserDto.User.PhoneNumber,
                            userAddAjaxModel.UserDto.User.About.length > 75 ? userAddAjaxModel.UserDto.User.About.substring(0, 75) : userAddAjaxModel.UserDto.User.About,
                            `
                                <img src="/img/${userAddAjaxViewModel.UserDto.User.Picture}" alt="picture" style="max-width: 50px;">
                            `,
                            `
                                <button class="btn btn-info btn-sm btn-detail" data-id="${userAddAjaxModel.UserDto.User.Id}"><span class="fas fa-newspaper"></span></button>
                                <button class="btn btn-warning btn-sm btn-assign" data-id="${userAddAjaxModel.UserDto.User.Id}"><span class="fas fa-user-shield"></span></button>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${userAddAjaxModel.UserDto.User.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${userAddAjaxModel.UserDto.User.Id}"><span class="fas fa-minus-circle"></span></button>
                            `
                        ]).node();
                        const jQueryTableRow = $(tableRow);
                        jQueryTableRow.attr("data-id",`user-row-${userAddAjaxViewModel.UserDto.User.Id}`)
                        dataTable.row(tableRow).draw();
                        toastr.success(userAddAjaxViewModel.UserDto.Message);
                    }
                },
                fail: function(err){
                    console.log(err);
                }
            })
        })
        /* Post request for Add method ends here */

        /* Post request for Delete method starts here */
        $(document).on("click",".btn-delete",function(e){
            e.preventDefault();
            var userId = $(this).attr("data-id");
            var tableRow = $(`[data-id="user-row-${userId}"]`);
            var userName = tableRow.find("td:eq(1)").text()

            Swal.fire({
                title: 'Are you sure that you want to delete?',
                text: `${userName} will be deleted!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: "No, don't delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'POST',
                        url: '/Admin/User/Delete/',
                        dataType: 'json',
                        data: { userId: userId },
                        success: function(data){
                            var userDto = jQuery.parseJSON(data);
                            if(userDto.ResultStatus === 0){
                                Swal.fire(
                                    'Deleted!',
                                    `${userName} has been deleted.`,
                                    'success'
                                )
                                dataTable.row(tableRow).remove().draw();
                            }else{
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `${user.Message}`,
                                });
                            }
                        },
                        error: function(err){
                            toastr.error(err.responseText);
                        }
                  })
                }
            })
        })
        /* Post request for Delete method ends here */

    })

    $(function () {

        /* Get request for Update method starts here */
        const modalPlaceHolder = $("#modalPlaceHolder");
        $("#usersTable").on("click",".btn-update",function(e){
            e.preventDefault();
            var userId = $(this).attr("data-id");
            var url = "/Admin/User/Update/";
            $.get(url,{ userId: userId }).done(function(data){
                modalPlaceHolder.html(data);
                modalPlaceHolder.find(".modal").modal("show");
            }).fail(function(){
                toastr.error("Error");
            })
        })
        /* Get request for Update method ends here */

        /* Post request for Update method starts here */
        modalPlaceHolder.on("click","#btnUpdate",function(e){
            e.preventDefault();
            var form = $("#user-update-form");
            var actionUrl = form.attr("action");
            var dataToSend = new FormData(form.get(0));
            $.ajax({
                type: "POST",
                url: actionUrl,
                data: dataToSend,
                processData: false,
                contentType: false,
                success: function(data){
                    const userUpdateAjaxViewModel = jQuery.parseJSON(data);
                    var newModalBody = $(".modal-body",userUpdateAjaxViewModel.UserUpdatePartial);
                    modalPlaceHolder.find(".modal-body").replaceWith(newModalBody);
                    var IsValid = newModalBody.find("[name='IsValid']").val() === "True";
                    if(IsValid){
                        var userId = userUpdateAjaxViewModel.UserDto.User.Id;
                        var tableRow = $("#usersTable").find(`[data-id="user-row-${userId}"]`);
                        modalPlaceHolder.find(".modal").modal("hide");
                        dataTable.row(tableRow).data([
                            userUpdateAjaxModel.UserDto.User.Id,
                            userUpdateAjaxModel.UserDto.User.UserName,
                            userUpdateAjaxModel.UserDto.User.Email,
                            userUpdateAjaxModel.UserDto.User.FirstName,
                            userUpdateAjaxModel.UserDto.User.LastName,
                            userUpdateAjaxModel.UserDto.User.PhoneNumber,
                            userUpdateAjaxModel.UserDto.User.About.length > 75 ? userUpdateAjaxModel.UserDto.User.About.substring(0, 75) : userUpdateAjaxModel.UserDto.User.About,
                            `<img src="/img/${userUpdateAjaxModel.UserDto.User.Picture}" alt="${userUpdateAjaxModel.UserDto.User.UserName}" class="my-image-table" />`,
                            `
                                <button class="btn btn-info btn-sm btn-detail" data-id="${userUpdateAjaxModel.UserDto.User.Id}"><span class="fas fa-newspaper"></span></button>
                                <button class="btn btn-warning btn-sm btn-assign" data-id="${userUpdateAjaxModel.UserDto.User.Id}"><span class="fas fa-user-shield"></span></button>
                                <button class="btn btn-primary btn-sm btn-update" data-id="${userUpdateAjaxModel.UserDto.User.Id}"><span class="fas fa-edit"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${userUpdateAjaxModel.UserDto.User.Id}"><span class="fas fa-minus-circle"></span></button>
                            `
                        ])
                        tableRow.attr("data-id",`user-row-${userId}`)
                        dataTable.row(tableRow).invalidate();
                        toastr.success(userUpdateAjaxViewModel.UserDto.Message);
                    }else{
                        let summaryText = "";
                        $("#validation-summary > ul > li").each(function(){
                            const text = $(this).text();
                            summaryText += `*${text}/n`;
                        })
                        toastr.warning(`${summaryText}`);
                    }
                },
                error: function(err){
                    console.log(err);
                }
            })
        })
        /* Post request for Update method ends here */
    })

    $(function () {
        /* Get request for detail starts here */
        const url = '/Admin/User/GetDetail/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-detail',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { userId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function (err) {
                    toastr.error(`${err.responseText}`, 'Error!');
            });
        });
        /* Get request for detail ends here */
    })

    $(function () {
        const url = '/Admin/Role/Assign/';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-assign',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { userId: id }).done(function (data) {
                    console.log("sa")
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function (err) {
                    toastr.error(`${err.responseText}`, 'Error!');
                });
            });

        /* Ajax POST / Updating a Comment starts from here */

        placeHolderDiv.on('click',
            '#btnAssign',
            function (event) {
                event.preventDefault();
                const form = $('#form-role-assign');
                const actionUrl = form.attr('action');
                const dataToSend = new FormData(form.get(0));
                console.log(actionUrl)
                $.ajax({
                    url: actionUrl,
                    type: 'POST',
                    data: dataToSend,
                    processData: false,
                    contentType: false,
                    success: function (data) {
                        const userRoleAjaxViewModel = jQuery.parseJSON(data);
                        //if (commentUpdateAjaxModel) {
                        //    const id = commentUpdateAjaxModel.CommentDto.Comment.Id;
                        //    const tableRow = $(`[name="${id}"]`);
                        //}
                        const id = userRoleAjaxViewModel.UserDto.User.Id;
                        const tableRow = $(`[name="${id}"]`);
                        const newFormBody = $('.modal-body', userRoleAjaxViewModel.RoleAssignPartial);
                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            placeHolderDiv.find('.modal').modal('hide');
                            tableRow.attr("name", `${id}`);
                            dataTable.row(tableRow).invalidate();
                            toastr.success(`${userRoleAjaxViewModel.UserDto.Message}`, "Successfull!");
                        } else {
                            let summaryText = "";
                            $('#validation-summary > ul > li').each(function () {
                                let text = $(this).text();
                                summaryText = `*${text}\n`;
                            });
                            toastr.warning(summaryText);
                        }
                    },
                    error: function (error) {
                        console.log(error);
                        toastr.error(`${error.responseText}`, 'Error!');
                    }
                });
            });

    });
} );