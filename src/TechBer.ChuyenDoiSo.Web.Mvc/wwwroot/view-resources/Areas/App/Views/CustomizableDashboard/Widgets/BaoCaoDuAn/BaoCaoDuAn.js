$(function () {
    var _hostDashboardService = abp.services.app.hostDashboard;
    var _widgetBase = app.widgetBase.create();
    var _$Container = $('.BaoCaoDuAnContainer');

    var initBaoCaoLoaiDuAn = function (datas) {
    //doughnut amchart
        var labels = datas.map(p => p.tenLoaiDuAn);
        console.log('9',labels);
        var dulieus= datas.map(p => p.loaiDuAn_SoDuAn);
        var backColor = [];
        
        var html = '<table class="table table-bordered table-striped" style="padding: 10px">' +
                        '<thead>' +
                            '<tr>' +
                                '<th style="text-align: center; font-weight: bold">STT</th>' + 
                                '<th style="text-align: center; font-weight: bold">Loại dự án</th>' + 
                                '<th style="text-align: center; font-weight: bold">Số dự án</th>' + 
                            '</tr>' +
                        '</thead>' +
                        '<tbody>';
        
        var stt = 1;
        var demMau = 0;
        var demMang = 0;
        $.each(datas, function () {
           var color = 'rgb('+ Math.floor(Math.random() * 255-demMau) +',' +
               Math.floor(Math.random() * 255-demMau) + ',' +
               Math.floor(Math.random() * 255-demMau) + ')';
           backColor.push(color);
            demMau +=5;
           
           html += '<tr>' + 
                        '<td>'+ stt + '</td>' +
                        '<td>'+ datas[demMang].tenLoaiDuAn + '</td>' +
                        '<td>'+ datas[demMang].loaiDuAn_SoDuAn + '</td>' +
                    '</tr>';
           stt++;
           demMang++;
        });
        const chartData = {
            labels: labels,
            datasets: [{
                label: 'My First Dataset',
                data: dulieus,
                backgroundColor: backColor ,
                hoverOffset: 4
            }]
        };
        var canvas = $(_$Container[0]).find('#baoCaoLoaiDuAn');

        var myChart = new Chart(canvas, {
            type: 'doughnut',
            data: chartData
        });
    //chi tiết
       html += '</tbody></table>';
       $('#chiTietBaoCaoLoaiDuAn').html(html)
    };
   



    var getBaoCao = function () {
        abp.ui.setBusy(_$Container);

        _hostDashboardService
            .getBaoCaoDuAn()
            .done(function (result) {
                initBaoCaoLoaiDuAn(result.listBaoCaoLoaiDuAns)
            }).always(function () {
            abp.ui.clearBusy(_$Container);
        });
    };

    _widgetBase.runDelayed(function () {
        getBaoCao();
    });
});