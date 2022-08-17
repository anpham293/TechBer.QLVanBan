$(function () {
    var _tenantDashboardService = abp.services.app.tenantDashboard;
    var _widgetBase = app.widgetBase.create();
    var _$ContainerDoiTuong = $('.BaoCaoDoiTuongContainer');
    var doiTuongTableBody = $('#bao-cao-doi-tuong > tbody')

    const configTheoDoSau = {
        0: {fontSize: '14px;', fontWeight: 'bold;', textIndent: '0;'},
        1: {fontSize: '12px;', fontWeight: 'normal;', textIndent: '15px;'}
    }

    $(document).ready(function () {
        var select2BaoCaoDoiTUong = $('#DoiTuongFilterId')

        select2BaoCaoDoiTUong.select2({
            placeholder: 'Select',
            ajax: {
                url: abp.appPath + "api/services/app/DoiTuongChuyenDoiSos/getAllDoiTuongChuyenDoiSo",
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        searchTerm: params.term, // search term
                    };
                },
                processResults: function (data, params) {
                    return {
                        results: $.map(data.result, function (item) {
                            return {
                                text: item.displayName,
                                id: item.id
                            }
                        })
                    };
                },
                cache: true
            },
            width: '100%',
            minimumResultsForSearch: 2,
        });

        select2BaoCaoDoiTUong.on("change", function (e) {
            _widgetBase.runDelayed(function () {
                getBaoCaoDoiTuong(select2BaoCaoDoiTUong.select2('data')[0].id);
            });
        });

    })

    // fill data vao bao cao
    var initBaoCaoChamDiemDoiTuong = function (data) {
        data.forEach(item => {
            doiTuongTableBody.append(partialRowDoiTuong(item));
        })
    }

    var partialRowDoiTuong = function (rowData) {
        let row = '<tr>' +
            '<td style = "font-weight: bold; text-align: left; font-size:' + configTheoDoSau[rowData['doSau']].fontSize + ' text-indent: ' + configTheoDoSau[rowData['doSau']].textIndent + '">' + rowData['tenTieuChi'] + '</td>' +
            '<td style = "font-weight: ' + configTheoDoSau[rowData['doSau']].fontWeight + ' font-size:' + configTheoDoSau[rowData['doSau']].fontSize + '">' + rowData['diemTuDanhGia'] + '</td>' +
            '<td style = "font-weight: ' + configTheoDoSau[rowData['doSau']].fontWeight + ' font-size:' + configTheoDoSau[rowData['doSau']].fontSize + '">' + rowData['diemHoiDongThamDinh'] + '</td>' +
            '<td style = "font-weight: ' + configTheoDoSau[rowData['doSau']].fontWeight + ' font-size:' + configTheoDoSau[rowData['doSau']].fontSize + '">' + rowData['diemDatDuoc'] + '</td>' +
            '</tr>';
        return row
    }

    var getBaoCaoDoiTuong = function (idDoiTuong) {
        abp.ui.setBusy(_$ContainerDoiTuong);

        doiTuongTableBody.html('');

        _tenantDashboardService
            .getBaoCaoChamDiemDoiTuong(idDoiTuong)
            .done(function (result) {
                initBaoCaoChamDiemDoiTuong(result)
            }).always(function () {
                abp.ui.clearBusy(_$ContainerDoiTuong);
            });
    };

    _widgetBase.runDelayed(function () {
        getBaoCaoDoiTuong();
    });

});