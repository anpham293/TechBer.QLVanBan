$(function () {
    var _tenantDashboardService = abp.services.app.tenantDashboard;
    var _widgetBase = app.widgetBase.create();
    var _$Container = $('.BaoCaoChamDiemContainer');
    var tableBody = $('#bao-cao-tong-hop > tbody')

    var PHAN_NHOM_FIELD_NAME = [
        'thongTinChung',
        'nhanThucSo',
        'theCheSo',
        'haTangSo',
        'nhanLucSo',
        'anToanThongTinMang',
        'hoatDongChinhQuyenSo',
        'hoatDongKinhTeSo',
        'hoatDongXaHoiSo',
        'doThiThongMinh',
        'thongTinVaDuLieuSo',
    ]

    var dichSangTiengViet = {
        'thongTinChung': 'Thông tin chung',
        'nhanThucSo': 'Nhận thức số',
        'theCheSo': 'Thể chế số',
        'haTangSo': 'Hạ tầng số',
        'nhanLucSo': 'Nhân lực số',
        'anToanThongTinMang': 'An toàn thông tin mạng',
        'hoatDongChinhQuyenSo': 'Hoạt động chính quyền số',
        'hoatDongKinhTeSo': 'Hoạt động kinh tế số',
        'hoatDongXaHoiSo': 'Hoạt động xã hội số',
        'doThiThongMinh': 'Đô thị thông minh',
        'thongTinVaDuLieuSo': 'Thông tin và dữ liệu số'
    }

    var initBaoCaoTongHop = function (data) {
        let rowDatas = {};
        let tongDiemDatDuoc = 0;
        let tongDiemHoiDongThamDinh = 0;
        let tongDiemTuDanhGia = 0;

        PHAN_NHOM_FIELD_NAME.forEach(item => {
            rowDatas[item] = {};

            rowDatas[item].diemDatDuoc = data.diemDatDuoc[item].toFixed(3);
            tongDiemDatDuoc += parseFloat(data.diemDatDuoc[item].toFixed(3));

            rowDatas[item].diemHoiDongThamDinh = data.diemHoiDongThamDinh[item].toFixed(3);
            tongDiemHoiDongThamDinh += parseFloat(data.diemHoiDongThamDinh[item].toFixed(3));

            rowDatas[item].diemTuDanhGia = data.diemTuDanhGia[item].toFixed(3);
            tongDiemTuDanhGia += parseFloat(data.diemTuDanhGia[item].toFixed(3));
        });

        PHAN_NHOM_FIELD_NAME.forEach(item => {
            tableBody.append(partialRow(item, rowDatas[item]));
        })

        tableBody.append('<tr>' +
            '<td style="font-weight: bold">Tổng</td>' +
            '<td style="font-weight: bold">' + tongDiemTuDanhGia.toFixed(3) + '</td>' +
            '<td style="font-weight: bold">' + tongDiemHoiDongThamDinh.toFixed(3) + '</td>' +
            '<td style="font-weight: bold">' + tongDiemDatDuoc.toFixed(3) + '</td>' +
        '</tr>')
    }

    var partialRow = function (nhom, dataObject) {
        let row = '<tr>' +
                '<td style="font-weight: bold; text-align: left">' + dichSangTiengViet[nhom] + '</td>' +
                '<td>' + dataObject.diemTuDanhGia + '</td>' +
                '<td>' + dataObject.diemHoiDongThamDinh + '</td>' +
                '<td>' + dataObject.diemDatDuoc + '</td>' +
            '</tr>';
        return row
    }

    var getBaoCao = function () {
        abp.ui.setBusy(_$Container);

        _tenantDashboardService
            .getBaoCaoTongHop()
            .done(function (result) {
                console.log(result);
                initBaoCaoTongHop(result)
            }).always(function () {
                abp.ui.clearBusy(_$Container);
            });
    };

    _widgetBase.runDelayed(function () {
        getBaoCao();
    });
});