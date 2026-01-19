$(function () {

    // 全現場チェック時、現場ドロップダウン無効化
    $('#allSiteChk').change(function () {
        if ($(this).is(':checked')) {
            $('#siteDropdown').prop('disabled', true).val('');
        } else {
            $('#siteDropdown').prop('disabled', false);
        }
    });

    // Client-side validation
    $('#exportForm').submit(function () {
        let isAllSite = $('#allSiteChk').is(':checked');
        let siteId = $('#siteDropdown').val();
        let startDate = $('#startDate').val();
        let endDate = $('#endDate').val();

        if (!isAllSite && !siteId) {
            alert('対象現場を選択してください。');
            return false;
        }

        if (!startDate || !endDate) {
            alert('申請日を入力してください。');
            return false;
        }

        if (startDate > endDate) {
            alert('申請日の範囲が正しくありません。');
            return false;
        }

        return true;
    });
});
