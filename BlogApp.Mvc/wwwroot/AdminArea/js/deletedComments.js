$(document).ready(function () {

    /* DataTables start here. */

    const dataTable = $('#deletedCommentsTable').DataTable({
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
                        url: '/Admin/Comment/GetAllDeletedComments/',
                        contentType: "application/json",
                        beforeSend: function () {
                            $('#deletedCommentsTable').hide();
                            $('.spinner-border').show();
                        },
                        success: function (data) {
                            const commentResult = jQuery.parseJSON(data);
                            dataTable.clear();
                            console.log(commentResult);
                            if (commentResult.Data) {
                                const articlesArray = [];
                                $.each(commentResult.Data.Comments.$values,
                                    function (index, comment) {
                                        const newComment = getJsonNetObject(comment, commentResult.Data.Comments.$values);
                                        let newArticle = getJsonNetObject(newComment.Article, newComment);
                                        if (newArticle !== null) {
                                            articlesArray.push(newArticle);
                                        }
                                        if (newArticle === null) {
                                            newArticle = articlesArray.find((article) => {
                                                return article.$id === newComment.Article.$ref;
                                            });
                                        }
                                        const newTableRow = dataTable.row.add([
                                            newComment.Id,
                                            newArticle.Title,
                                            newComment.Text.length > 75 ? newComment.Text.substring(0, 75) : newComment.Text,
                                            `${newComment.IsActive ? "Yes" : "No"}`,
                                            `${newComment.IsDeleted ? "Yes" : "No"}`,
                                            `${convertToShortDate(newComment.CreatedDate)}`,
                                            newComment.CreatedByName,
                                            `${convertToShortDate(newComment.ModifiedDate)}`,
                                            newComment.ModifiedByName,
                                            `
                                <button class="btn btn-warning btn-sm btn-undo" data-id="${newComment.Id}"><span class="fas fa-undo"></span></button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${newComment.Id}"><span class="fas fa-minus-circle"></span></button>
                                            `
                                        ]).node();
                                        const jqueryTableRow = $(newTableRow);
                                        jqueryTableRow.attr('name', `${newComment.Id}`);
                                    });
                                dataTable.draw();
                                $('.spinner-border').hide();
                                $('#deletedCommentsTable').fadeIn(1400);
                            } else {
                                toastr.error(`${commentResult.Message}`, 'Error!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#deletedCommentsTable').fadeIn(1000);
                            toastr.error(`${err.responseText}`, 'Error!');
                        }
                    });
                }
            }
        ]
    });

    /* DataTables end here */

    /* Ajax POST / Deleting a Comment starts from here */

    $(document).on('click',
        '.btn-delete',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            let commentText = tableRow.find('td:eq(2)').text();
            commentText = commentText.length > 75 ? commentText.substring(0, 75) : commentText;
            Swal.fire({
                title: 'Are you sure you want to delete from database?',
                text: `${commentText} will be deleted from database!`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, delete it!.',
                cancelButtonText: "Hayır, don't delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        data: { commentId: id },
                        url: '/Admin/Comment/HardDelete/',
                        success: function (data) {
                            const commentResult = jQuery.parseJSON(data);
                            console.log(commentResult);
                            if (commentResult.ResultStatus===0) {
                                Swal.fire(
                                    'Deleted!',
                                    `${commentResult.Message}`,
                                    'success'
                                );

                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error!',
                                    text: `${commentResult.Message}`,
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

    /* Ajax POST / Deleting a Comment ends here */


    /* Ajax POST / UndoDeleting a Comment starts from here */

    $(document).on('click',
        '.btn-undo',
        function (event) {
            event.preventDefault();
            const id = $(this).attr('data-id');
            const tableRow = $(`[name="${id}"]`);
            let commentText = tableRow.find('td:eq(2)').text();
            commentText = commentText.length > 75 ? commentText.substring(0, 75) : commentText;
            Swal.fire({
                title: 'Are you sure you want to undo?',
                text: `${commentText} will be brought back!`,
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, undo it.',
                cancelButtonText: "No, don't undo it."
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        data: { commentId: id },
                        url: '/Admin/Comment/UndoDelete/',
                        success: function (data) {
                            const commentResult = jQuery.parseJSON(data);
                            console.log(commentResult);
                            if (commentResult.Data) {
                                Swal.fire(
                                    'Bring Back!',
                                    `Comment it's id ${commentResult.Data.Comment.Id} bring back.`,
                                    'success'
                                );
                                dataTable.row(tableRow).remove().draw();
                            } else {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error!',
                                    text: `Error.`,
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
    /* Ajax POST / UndoDeleting a Comment starts ends here */
});