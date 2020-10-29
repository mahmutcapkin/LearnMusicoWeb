$(function () {

    var sharingids = [];
    $("div[data-sharing-id]").each(function (i, e) {
        sharingids.push($(e).data("sharing-id"));
    });

    $.ajax({
        method: "POST",
        url: "/Sharing/GetLiked",
        data: { ids: sharingids }

    }).done(function (data) {

        if (data.result != null && data.result.length > 0) {
            for (var i = 0; i < data.result.length; i++) {
                var id = data.result[i];
                var likedSharing = $("div[data-sharing-id=" + id + "]");
                var btn = likedSharing.find("button[data-liked]");
                var span = btn.children().first();

                btn.data("liked", true);
                span.removeClass("far fa-heart");
                span.addClass("fas fa-heart");
            }
        }


    }).fail(function () {

    });


    $("button[data-liked]").click(function () {
        var btn = $(this);
        var liked = btn.data("liked");
        var sharingid = btn.data("sharing-id");
        var spanStar = btn.find("i.like-star");
        var spanCount = btn.find("span.like-count");

        $.ajax({
            method: "POST",
            url: "/Sharing/LikedState",
            data: { "sharingid": sharingid, "liked": !liked }
        }).done(function (data) {

            if (data.hasError) {
                alert(data.errorMessage);
            }
            else {
                liked = !liked;
                btn.data("liked", liked);
                spanCount.text(data.result);

                spanStar.removeClass("far");
                spanStar.removeClass("fas");

                if (liked) {
                    spanStar.addClass("fas");
                }
                else {
                    spanStar.addClass("far");
                }
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        });

    });


});