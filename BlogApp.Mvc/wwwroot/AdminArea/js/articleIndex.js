$(document).ready( function () {
    const dataTable = $('#articlesTable').DataTable({
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
                }
            },
            {
                text: 'Refresh',
                className: "btn btn-warning",
                action: function ( e, dt, node, config ) {
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/Article/GetAll/',
                        contentType: 'application/json',
                        beforeSend: function(){
                            $("#spinner").show();
                            $("#articlesTable").hide();
                        },
                        success: function (data) {
                            const articleResult = jQuery.parseJSON(data);
                            if (articleResult.Data.ResultStatus === 0) {
                                let categoriesArray = [];
                                $.each(articleResult.Data.Articles.$values,function(index,article){
                                    let newArticle = getJsonNetObject(article, articleResult.Data.Articles.$values);
                                    let newCategory = getJsonNetObject(newArticle.Category, newArticle);
                                    if (newCategory !== null) {
                                        categoriesArray.push(newCategory)
                                    }
                                    if (newCategory === null) {
                                        newCategory = categoriesArray.find((category) => {
                                            return category.$id === newArticle.Category.$ref;
                                        })
                                    }

                                    const newTableRow = dataTable.row.add([
                                        newArticle.Id,
                                        newCategory.Name,
                                        newArticle.Title,
                                        `<img src="/img/PostImages/${newArticle.Thumbnail}" alt="${newArticle.Title}" class="my-image-table" />`,
                                        `${ConvertToShortDate(newArticle.Date)}`,
                                        newArticle.ViewCount,
                                        newArticle.CommentCount,
                                        `${newArticle.IsActive ? "Yes" : "No"}`,
                                        `${newArticle.IsDeleted ? "Yes" : "No"}`,
                                        `${ConvertToShortDate(newArticle.CreatedDate)}`,
                                        newArticle.CreatedByName,
                                        `${ConvertToShortDate(newArticle.ModifiedDate)}`,
                                        newArticle.ModifiedByName,
                                        `
                                        <button class="btn btn-primary btn-sm btn-update" data-id="${newArticle.Id}"><span class="fas fa-edit"></span></button>
                                        <button class="btn btn-danger btn-sm btn-delete" data-id="${newArticle.Id}"><span class="fas fa-minus-circle"></span></button>
                                            `
                                    ]).node();
                                    
                                    const jqueryTableRow = $(newTableRow);
                                    jqueryTableRow.attr('data-id', `article-row-${newArticle.Id}`);
                                    
                                });
                                
                                dataTable.draw();
                                $("#articlesTable").fadeIn(2000);
                                $("#spinner").hide();

                                toastr.success(`${articleResult.Message}`,"Successfull")
                            }
                            else{
                                $("#articlesTable").fadeIn(1000);
                                $("#spinner").hide();
                                toastr.error(`${articleResult.Message}`,"Error")
                            }
                        },
                        error: function(err){
                            $("#articlesTable").fadeIn(1000);
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
            const articleId = $(this).attr("data-id");
            const tableRow = $(`[data-id="article-row-${articleId}"]`);
            const articleName = tableRow.find("td:eq(2)").text();

            Swal.fire({
                title: 'Are you sure that you want to delete?',
                text: `${articleName} will be deleted!`,
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
                        data: { articleId: articleId },
                        url: '/Admin/Article/Delete/',
                        success: function(data){
                            const articleResult = jQuery.parseJSON(data);
                            if(articleResult.ResultStatus == 0){
                                Swal.fire(
                                    'Deleted!',
                                    `${articleResult.Message}`,
                                    'success'
                                );
                                tableRow.fadeOut(2000);
                            }else{
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Oops...',
                                    text: `${articleResult.Message}`,
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


