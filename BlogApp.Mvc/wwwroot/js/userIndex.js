$(document).ready( function () {
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
                                        user.PhoneNumber,
                                        `
                                            <img src="/img/${user.Picture}" alt="picture" style="max-width: 50px;">
                                        `,
                                        `
                                            <button class="btn btn-warning btn-update btn-block" data-id=${user.Id}><span class="fa-solid fa-pen-to-square"></span></button>
                                            <button class="btn btn-danger btn-delete btn-block" data-id=${user.Id}><span class="fa-solid fa-circle-xmark"></span></button>
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

    $(function(){
        const modalPlaceHolder = $("#modalPlaceHolder");
        const url = '/Admin/User/Add/';
        $("#btnAdd").click(function(){
            $.get(url).done(function(data){
                modalPlaceHolder.html(data);
                modalPlaceHolder.find(".modal").modal("show");
            })
        })

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
                            userAddAjaxViewModel.UserDto.User.Id,
                            userAddAjaxViewModel.UserDto.User.UserName,
                            userAddAjaxViewModel.UserDto.User.Email,
                            userAddAjaxViewModel.UserDto.User.PhoneNumber,
                            `
                                <img src="/img/${userAddAjaxViewModel.UserDto.User.Picture}" alt="picture" style="max-width: 50px;">
                            `,
                            `
                                <button class="btn btn-warning btn-update btn-block" data-id=${userAddAjaxViewModel.UserDto.User.Id}><span class="fa-solid fa-pen-to-square"></span></button>
                                <button class="btn btn-danger btn-delete btn-block" data-id=${userAddAjaxViewModel.UserDto.User.Id}><span class="fa-solid fa-circle-xmark"></span></button>
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

        $(document).on("click",".btn-delete",function(e){
            e.preventDefault();
            var userId = $(this).attr("data-id");
            var tableRow = $("#usersTable").find(`[data-id="user-row-${userId}"]`);
            var userName = tableRow.find("td:eq(1)").val();
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
    })

    $(function(){
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
                            userUpdateAjaxViewModel.UserDto.User.Id,
                            userUpdateAjaxViewModel.UserDto.User.UserName,
                            userUpdateAjaxViewModel.UserDto.User.Email,
                            userUpdateAjaxViewModel.UserDto.User.PhoneNumber,
                            `
                                <img src="/img/${userUpdateAjaxViewModel.UserDto.User.Picture}" alt="picture" style="max-width: 50px;">
                            `,
                            `
                                <button class="btn btn-warning btn-update btn-block" data-id=${userUpdateAjaxViewModel.UserDto.User.Id}><span class="fa-solid fa-pen-to-square"></span></button>
                                <button class="btn btn-danger btn-delete btn-block" data-id=${userUpdateAjaxViewModel.UserDto.User.Id}><span class="fa-solid fa-circle-xmark"></span></button>
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
    })
} );