// Runs after page is loaded
$(function () {
    console.log("Site.js loaded");

    // Global AJAX error handler (VERY useful)
    $(document).ajaxError(function (event, xhr) {
        alert("エラーが発生しました: " + xhr.status);
    });
});

// Confirm delete (used everywhere)
function confirmDelete() {
    return confirm("削除してもよろしいですか？");
}
