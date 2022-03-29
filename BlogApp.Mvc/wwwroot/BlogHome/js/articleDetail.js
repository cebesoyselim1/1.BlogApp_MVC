$(document).ready(function () {
    $(function () {
        $(document).on("click", "#btnSave", function (e) {
            e.preventDefault();
            const form = $("#form-comment-add");
            const actionUrl = form.attr("action");
            const dataToSend = form.serialize();
            $.post(actionUrl, dataToSend).done(function (data) {
                const commentAjaxViewModel = jQuery.parseJSON(data);
                const newFormBody = $(".form-card", commentAjaxViewModel.CommentPartial);
                const cardBody = $(".card-body");
                cardBody.replaceWith(newFormBody);
                const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                if (isValid) {
                    const newComment = `
                    <div class="media mb-4">
                        <img class="d-flex mr-3 rounded-circle" src="/img/UserImages/defaultUser.png" style="max-width: 100px;">
                        <div class="media-body">
                            <h5 class="mt-0">${commentAjaxViewModel.CommentDto.Comment.CreatedByName}</h5>
                            ${commentAjaxViewModel.CommentDto.Comment.Text}
                        </div>
                    </div>
                    `;
                    const newCommentObject = $(newComment);
                    newCommentObject.hide();
                    $("#comments").append(newCommentObject);
                    newCommentObject.fadeIn(2000);
                    toastr.success(`Your message has successfully been added. In order to see your comment, wait for approvement. Comment you see now is just an exapmle.`);
                    $("#btnSave").prop("disabled", true);
                    setTimeout(function () {
                        $("#btnSave").prop("disabled", false);
                    },10000)
                } else {
                    let summaryText = "";
                    $('#validation-summary > ul > li').each(function () {
                        let text = $(this).text();
                        summaryText += `*${text}\n`;
                    });
                    toastr.warning(summaryText);
                }
            }).fail(function (err) {
                console.log(err);
            })
        })
    })
})