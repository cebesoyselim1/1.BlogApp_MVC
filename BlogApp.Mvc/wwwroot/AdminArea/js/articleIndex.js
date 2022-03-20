$(document).ready( function () {
    $('#articlesTable').DataTable({
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
                action: function (e, dt, node, config) {
                    let url = window.location.href;
                    url = url.replace("/Index", "");
                    window.open(`${url}/Add`, "_self");
                    window.open()
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
});