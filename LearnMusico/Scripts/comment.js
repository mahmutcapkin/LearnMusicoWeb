var sharingid = -1;
var modalCommentBodyId = "#modal_comment_body";

$(function () {
    $('#modal_comment').on('show.bs.modal', function (e) {
        var btne = $(e.relatedTarget);
        sharingid = btne.data("sharing-id");
        $(modalCommentBodyId).load("/Comment/ShowSharingComments/" + sharingid);

    })
});

function doComment(btn, e, commentid, spanid) {

    var button = $(btn);
    var mode = button.data("edit-mode");

    if (e === "edit_clicked") {
        if (!mode) {
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnI = button.find("i");
            btnI.removeClass("fa-edit");
            btnI.addClass("fa-check-circle");

            $(spanid).addClass("editable");
            $(spanid).attr("contenteditable", true);
        }
        else {
            button.data("edit-mode", false);
            button.addClass("btn-warning");
            button.removeClass("btn-success");
            var btnI = button.find("i");
            btnI.addClass("fa-edit");
            btnI.removeClass("fa-check-circle");

            $(spanid).removeClass("editable");
            $(spanid).attr("contenteditable", false);
            $(spanid).focus();

            var txt = $(spanid).text();

            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentid,
                data: { text: txt }
            }).done(function (data) {
                if (data.result) {
                    //yorumlar partial tekrar yüklenir..
                    $(modalCommentBodyId).load("/Comment/ShowSharingComments/" + sharingid);
                }
                else {
                    alert("Yorum Güncellenemedi.");
                }
            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı.");
            });
        }

    }
    else if (e === "delete_clicked") {
        var dialog_res = confirm("Yorum Silinsin mi ?");
        if (!dialog_res) return false;

        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid,

        }).done(function (data) {
            if (data.result) {
                //yorumlar partial tekrar yüklenir..
                $(modalCommentBodyId).load("/Comment/ShowSharingComments/" + sharingid);
            }
            else {
                alert("Yorum Silinemedi.");
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        });
    }
    else if (e === "new_clicked") {
        var txt = $("#new_comment_text").val();

        $.ajax({
            method: "POST",
            url: "/Comment/Create",
            data: { "text": txt, "sharingid": sharingid }

        }).done(function (data) {
            if (data.result) {
                //yorumlar partial tekrar yüklenir..
                $(modalCommentBodyId).load("/Comment/ShowSharingComments/" + sharingid);
            }
            else {
                alert("Yorum Eklenemedi.");
            }
        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        });
    }
}