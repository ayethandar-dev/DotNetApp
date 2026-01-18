let currentUkebaraiboId = '';
let currentSiteId = '';

$(function () {

    // ===== 添付 =====
    $('.btn-attach').click(function () {
        let row = $(this).closest('tr');
        currentUkebaraiboId = row.data('ukebaraibo-id');
        currentSiteId = row.data('site-id');

        $('#pdfFileInput').val('');
        $('#pdfFileInput').click();
    });

    // when file selected
    $('#pdfFileInput').change(function () {
        let file = this.files[0];
        if (!file) return;

        let formData = new FormData();
        formData.append('file', file);
        formData.append('siteId', currentSiteId);
        formData.append('ukebaraiboId', currentUkebaraiboId);

        $.ajax({
            url: '/File/Upload',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                alert('PDFをアップロードしました');
                location.reload(); // simple & safe
            }
        });
    });

    // ===== 表示 =====
    $('.btn-preview').click(function () {
        let row = $(this).closest('tr');
        let siteId = row.data('site-id');
        let ukebaraiboId = row.data('ukebaraibo-id');

        window.open(
            '/File/Preview?siteId=' + siteId + '&ukebaraiboId=' + ukebaraiboId,
            '_blank'
        );
    });

    // ===== 削除 =====
    $('.btn-delete').click(function () {
        if (!confirm('PDFを削除しますか？')) return;

        let row = $(this).closest('tr');
        let siteId = row.data('site-id');
        let ukebaraiboId = row.data('ukebaraibo-id');

        $.post('/File/Delete', {
            siteId: siteId,
            ukebaraiboId: ukebaraiboId
        }, function () {
            alert('PDFを削除しました');
            location.reload();
        });
    });

});

$(function () {

    // make site dropdown searchable
    $('#siteSelect').select2({
        width: '100%',
        placeholder: '現場を選択してください'
    });

    // reload page when site changes
    $('#siteSelect').change(function () {
        var siteId = $(this).val();
        if (!siteId) return;

        location.href = '/Ukebaraibo/Index?siteId=' + siteId;
    });
    $('.select-company').click(function () {
        let ukebaraiboId = $(this).closest('tr').data('id');

        $.get('/CorporateCompany/Search', { ukebaraiboId }, function (html) {
            $('#modalArea').html(html);
            $('#companyModal').modal('show');
        });
    });


});
