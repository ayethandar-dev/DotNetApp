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

function downloadExcel() {
    document.getElementById("loading").style.display = "block";

    fetch("/Ukebaraibo/ExportExcel", {
        method: "POST"
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Download failed");
            }
            return response.blob(); // 👈 VERY IMPORTANT
        })
        .then(blob => {
            const url = window.URL.createObjectURL(blob);

            const a = document.createElement("a");
            a.href = url;
            a.download = "CorporateMaisu.xlsx"; // filename (can be overwritten by server header)
            document.body.appendChild(a);
            a.click();

            window.URL.revokeObjectURL(url);
            a.remove();

            document.getElementById("loading").style.display = "none";
        })
        .catch(error => {
            alert("エクセル出力に失敗しました");
            console.error(error);
            document.getElementById("loading").style.display = "none";
        });
}

function downloadExcelAjax() {
    const siteId = document.getElementById("SiteId").value;
    const allSites = document.getElementById("AllSitesChk").checked;
    const startDateStr = document.getElementById("StartDate").value;
    const endDateStr = document.getElementById("EndDate").value;

    if (!allSites && !siteId) {
        alert("現場を選択してください（または全現場にチェック）");
        return;
    }

    if (!startDateStr || !endDateStr) {
        alert("開始日と終了日を入力してください");
        return;
    }

    const startDate = new Date(startDateStr);
    const endDate = new Date(endDateStr);

    if (startDate > endDate) {
        alert("開始日は終了日より前の日付を指定してください");
        return;
    }

    // ✅ 1年以内チェック
    const oneYearLater = new Date(startDate)
    oneYearLater.setFullYear(oneYearLater.getFullYear() + 1);

    if (endDate > oneYearLater) {
        alert("期間は最大1年以内で指定してください");
        return;
    }
    $("#loading").show();

    $.ajax({
        url: "/Ukebaraibo/ExportExcel",
        type: "POST",
        data: {
            siteId: $("#SiteId").val(),
            startDate: $("#StartDate").val(),
            endDate: $("#EndDate").val()
        },
        xhrFields: {
            responseType: "blob"   // 👈 VERY IMPORTANT
        },
        success: function (blob, status, xhr) {
            const filename = "CorporateMaisu.xlsx";

            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = filename;

            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);

            $("#loading").hide();
        },
        error: function () {
            alert("Excel download failed");
            $("#loading").hide();
        }
    });
}


