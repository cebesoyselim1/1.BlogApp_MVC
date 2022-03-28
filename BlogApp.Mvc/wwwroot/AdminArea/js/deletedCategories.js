$(document).ready(function () {

    /* DataTables start here. */

    const dataTable = $('#deletedCategoriesTable').DataTable({
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Refresh',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/Category/GetAllDeletedCategories/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#deletedCategoriesTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const deletedCategories = jQuery.parseJSON(data);
                            dataTable.clear();
                            console.log(deletedCategories);
                            if (deletedCategories.ResultStatus===0) {
                                $.each(deletedCategories.Categories.$values,
                                    function (index, category) {
                                        const newTableRow = dataTable.row.add([
                                            category.Id,
                                            category.Name,
                                            category.Description,
                                            category.IsActive ? "Yes" : "No",
                                            category.IsDeleted ? "Yes" : "No",
                                            category.Note,
                                            ConvertToShortDate(category.CreatedDate),
                                            category.CreatedByName,
                                            ConvertToShortDate(category.ModifiedDate),
                                            category.ModifiedByName,
                                            `
                                <button class="btn btn-warning btn-sm btn-undo" data-id="${category.Id}"><span class="fas fa-undo"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${category.Id}"><span class="fas fa-minus-circle"></span></button>
                            `
                                        ]).node();
                                        const jqueryTableRow = $(newTableRow);
                                        jqueryTableRow.attr('name', `${category.Id}`);
                                    });
                                dataTable.draw();
                                $('.spinner-border').hide();
                                $('#deletedCategoriesTable').fadeIn(1400);
                            } else {
                                toastr.error(`${deletedCategories.Categories.Message}`, 'Error!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#deletedCategoriesTable').fadeIn(1000);
                            toastr.error(`${err.responseText}`, 'Error!');
                        }
                    });
                }
            }
        ]
    });

    /* DataTables end here */

    /* Undo Delete */
    $(document).on('click',
        '.btn-undo',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[data-id="category-row-${id}"]`);
            let categoryName = tableRow.find('td:eq(1)').text();
            Swal.fire({
                title: 'Are you sure that you want to undo?',
                text: `${categoryName} will be brought back!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, undo it!',
                cancelButtonText: "No, don't undo it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        data: { categoryId: id },
                        url: '/Admin/Category/UndoDelete/',
                        success: function (data) {
                            console.log(data);
                            const undoDeletedCategoryResult = jQuery.parseJSON(data);
                            if (undoDeletedCategoryResult.ResultStatus === 0) {
                                Swal.fire(
                                    'Bring back!',
                                    `${undoDeletedCategoryResult.Message}`,
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error!',
                                    text: `${undoDeletedCategoryResult.Message}`,
                                });
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            toastr.error(`${err.responseText}`, "Error!");
                        }
                    });
                }
            });
        });
        
        /* Hard Delete */
        $(document).on('click',
        '.btn-delete',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[data-id="category-row-${id}"]`);
            let categoryName = tableRow.find('td:eq(1)').text();
            Swal.fire({
                title: 'Are you sure that you want to delete from database?',
                text: `${categoryName} will be deleted from database!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it from database!',
                cancelButtonText: "No, don't delete it from database!"
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        data: { categoryId: id },
                        url: '/Admin/Category/HardDelete/',
                        success: function (data) {
                            const hardDeleteResult = jQuery.parseJSON(data);
                            console.log(hardDeleteResult);
                            if (hardDeleteResult.ResultStatus === 0) {
                                Swal.fire(
                                    'Delete it!',
                                    `${hardDeleteResult.Message}`,
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error!',
                                    text: `${hardDeleteResult.Message}`,
                                });
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            toastr.error(`${err.responseText}`, "Error!");
                        }
                    });
                }
            });
        })

});