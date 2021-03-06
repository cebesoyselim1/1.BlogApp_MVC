$(document).ready( function () {
    $('#categoriesTable').DataTable({
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
                        url: '/Admin/Category/GetAll/',
                        contentType: 'application/json',
                        beforeSend: function(){
                            $("#spinner").show();
                            $("#categoriesTable").hide();
                        },
                        success: function(data){
                            const categoryAddDto = jQuery.parseJSON(data);
                            if(categoryAddDto.ResultStatus == 0){
                                let newTableBody = "";
                                $.each(categoryAddDto.Categories.$values,function(index,category){
                                    newTableBody += `
                                        <tr data-id="category-row-${category.id}">
                                            <td>${category.Id}</td>
                                            <td>${category.Name}</td>
                                            <td>${category.Description}</td>
                                            <td>${category.IsActive ? "Yes" : "No"}</td>
                                            <td>${category.IsDeleted ? "Yes" : "No"}</td>
                                            <td>${category.Note}</td>
                                            <td>${category.CreatedByName}</td>
                                            <td>${ConvertToShortDate(category.CreatedDate)}</td>
                                            <td>${category.ModifiedByName}</td>
                                            <td>${ConvertToShortDate(category.ModifiedDate)}</td>
                                            <td>
                                                <button class="btn btn-warning btn-block" data-id="${category.id}"><span class="fa-solid fa-pen-to-square"></span></button>
                                                <button class="btn btn-danger btn-delete btn-block" data-id="${category.id}"><span class="fa-solid fa-circle-xmark"></span></button>
                                            </td>
                                        </tr>
                                        `;
                                });
                                const newTableBodyObject = $(newTableBody);
                                $("#categoriesTable > tbody").replaceWith(newTableBodyObject)
                                $("#categoriesTable").fadeIn(2000);
                                $("#spinner").hide();

                                toastr.success(`${categoryAddDto.Message}`,"Successfull")
                            }
                            else{
                                $("#categoriesTable").fadeIn(1000);
                                $("#spinner").hide();
                                toastr.error(`${categoryAddDto.Message}`,"Error")
                            }
                        },
                        error: function(err){
                            $("#categoriesTable").fadeIn(1000);
                            $("#spinner").hide();
                            toastr.error(`${err.responseText}`,"Error")
                        }
                    })
                }
            }
        ]
    });

    $(function(){
        const modalPlaceHolder = $("#modalPlaceHolder");
        const url = '/Admin/Category/Add/';
        $("#btnAdd").click(function(){
            $.get(url).done(function(data){
                modalPlaceHolder.html(data);
                modalPlaceHolder.find(".modal").modal("show");
            })
        })

        modalPlaceHolder.on("click","#btnSave",function(e){
            e.preventDefault();
            const form = modalPlaceHolder.find("#category-add-form");
            const actionUrl = form.attr("action");
            const dataToSend = form.serialize();
            $.post(actionUrl,dataToSend).done(function(data){
                const categoryAddAjaxViewModel = jQuery.parseJSON(data);
                const newModelBody = $(".modal-body",categoryAddAjaxViewModel.CategoryAddPartial);
                modalPlaceHolder.find(".modal-body").replaceWith(newModelBody);
                const IsValid = newModelBody.find("#IsValid")[0].value == "True";
                if(IsValid){
                    modalPlaceHolder.find(".modal").modal("hide");
                    const newTableRow = `
                    <tr data-id="category-row-${categoryAddAjaxViewModel.CategoryDto.Category.Id}">
                        <td>${categoryAddAjaxViewModel.CategoryDto.Category.Id}</td>
                        <td>${categoryAddAjaxViewModel.CategoryDto.Category.Name}</td>
                        <td>${categoryAddAjaxViewModel.CategoryDto.Category.Description}</td>
                        <td>${categoryAddAjaxViewModel.CategoryDto.Category.IsActive ? "Yes" : "No"}</td>
                        <td>${categoryAddAjaxViewModel.CategoryDto.Category.IsDeleted ? "Yes" : "No"}</td>
                        <td>${categoryAddAjaxViewModel.CategoryDto.Category.Note}</td>
                        <td>${categoryAddAjaxViewModel.CategoryDto.Category.CreatedName}</td>
                        <td>${ConvertToShortDate(categoryAddAjaxViewModel.CategoryDto.Category.CreatedDate)}</td>
                        <td>${categoryAddAjaxViewModel.CategoryDto.Category.ModifiedName}</td>
                        <td>${ConvertToShortDate(categoryAddAjaxViewModel.CategoryDto.Category.ModifiedDate)}</td>
                        <td>
                            <button class="btn btn-warning btn-block" data-id="${categoryAddAjaxViewModel.CategoryDto.Category.Id}"><span class="fa-solid fa-pen-to-square"></span></button>
                            <button class="btn btn-danger btn-delete btn-block" data-id="${categoryAddAjaxViewModel.CategoryDto.Category.Id}"><span class="fa-solid fa-circle-xmark"></span></button>
                        </td>                                
                    </tr>
                    `;
                    
                    const newTableRowObject = $(newTableRow);
                    newTableRowObject.hide();
                    $("#categoriesTable").append(newTableRowObject);
                    newTableRowObject.fadeIn(1500);
                    toastr.success(`${categoryAddAjaxViewModel.CategoryDto.Category.Name} has successfully been added.`,"Succesfull");
                }else{
                    let summaryText = "";
                    $("#validation-summary > ul > li").each(function(){
                        const text = $(this).text();
                        summaryText += `*${text}/n`;
                    })
                    toastr.warning(`${summaryText}`);
                }
            })
        })
        $(document).on("click",".btn-delete",function(e){
            e.preventDefault();
            const categoryId = $(this).attr("data-id");
            const tableRow = $(`[data-id="category-row-${categoryId}"]`);
            const categoryName = tableRow.find("td:eq(1)").text();

            Swal.fire({
                title: 'Are you sure that you want to delete?',
                text: `${categoryName} will be deleted!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: "No, don't delete!"
                }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        data: { categoryId: categoryId },
                        url: '/Admin/Category/Delete/',
                        success: function(data){
                            const categoryDto = jQuery.parseJSON(data);
                            if(categoryDto.ResultStatus == 0){
                                Swal.fire(
                                    'Deleted!',
                                    `${categoryDto.Message}`,
                                    'success'
                                );
                                tableRow.fadeOut(2000);
                            }else{
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `${categoryDto.Message}`,
                                })
                            }
                        },
                        error: function(err){
                            Swal.fire({
                                icon: 'error',
                                title: 'Oops...',
                                text: `${err.responseText}`,
                            })
                        }
                    })
                }
            })
        })
    })

    $(function(){
        const modalPlaceHolder = $("#modalPlaceHolder");
        $("#categoriesTable").on("click",".btn-update",function(e){
            e.preventDefault();
            const url = "/Admin/Category/Update/";
            const categoryId = $(this).attr("data-id");
            $.get(url, { categoryId: categoryId }).done(function(data){
                modalPlaceHolder.html(data);
                modalPlaceHolder.find(".modal").modal("show");
            }).fail(function(){
                toastr.success(`${data.Message}`,"Error");
            })
        })

        modalPlaceHolder.on("click","#btnUpdate",function(e){
            e.preventDefault();
            const form = modalPlaceHolder.find("#category-update-form");
            const actionUrl = form.attr("action");
            const dataToSend = form.serialize();
            $.post(actionUrl,dataToSend).done(function(data){
                const categoryUpdateAjaxViewModel = jQuery.parseJSON(data);
                const newModalBody = $(".modal-body",categoryUpdateAjaxViewModel.CategoryUpdatePartial);
                modalPlaceHolder.find(".modal-body").replaceWith(newModalBody);
                const IsValid = newModalBody.find(`[name="IsValid"]`).val() === "True";
                if(IsValid){
                    modalPlaceHolder.find(".modal").modal("hide");
                    const newTableRow = `
                    <tr data-id="category-row-${categoryUpdateAjaxViewModel.CategoryDto.Category.Id}">
                        <td>${categoryUpdateAjaxViewModel.CategoryDto.Category.Id}</td>
                        <td>${categoryUpdateAjaxViewModel.CategoryDto.Category.Name}</td>
                        <td>${categoryUpdateAjaxViewModel.CategoryDto.Category.Description}</td>
                        <td>${categoryUpdateAjaxViewModel.CategoryDto.Category.IsActive ? "Yes" : "No"}</td>
                        <td>${categoryUpdateAjaxViewModel.CategoryDto.Category.IsDeleted ? "Yes" : "No"}</td>
                        <td>${categoryUpdateAjaxViewModel.CategoryDto.Category.Note}</td>
                        <td>${categoryUpdateAjaxViewModel.CategoryDto.Category.CreatedName}</td>
                        <td>${ConvertToShortDate(categoryUpdateAjaxViewModel.CategoryDto.Category.CreatedDate)}</td>
                        <td>${categoryUpdateAjaxViewModel.CategoryDto.Category.ModifiedName}</td>
                        <td>${ConvertToShortDate(categoryUpdateAjaxViewModel.CategoryDto.Category.ModifiedDate)}</td>
                        <td>
                            <button class="btn btn-warning btn-block" data-id="${categoryUpdateAjaxViewModel.CategoryDto.Category.Id}"><span class="fa-solid fa-pen-to-square"></span></button>
                            <button class="btn btn-danger btn-delete btn-block" data-id="${categoryUpdateAjaxViewModel.CategoryDto.Category.Id}"><span class="fa-solid fa-circle-xmark"></span></button>
                        </td>                                
                    </tr>
                    `;
                    const newTableRowObject = $(newTableRow);
                    const oldTableRow = $(`[data-id="category-row-${categoryUpdateAjaxViewModel.CategoryDto.Category.Id}"]`)
                    console.log(oldTableRow)
                    newTableRowObject.hide();
                    oldTableRow.replaceWith(newTableRowObject);
                    newTableRowObject.fadeIn(2000);
                    toastr.success(`${categoryUpdateAjaxViewModel.CategoryDto.Message}`,"Success");
                }else{
                    let summaryText = "";
                    $("#validation-summary > ul > li").each(function(){
                        const text = $(this).text();
                        summaryText += `*${text}/n`;
                    })
                    toastr.warning(`${summaryText}`);
                }
            }).fail(function(response){
                console.log(response);
            })
        })
    })
} );