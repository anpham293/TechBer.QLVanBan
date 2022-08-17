$(function () {
    var _tenantDashboardService = abp.services.app.tenantDashboard;
    var _widgetBase = app.widgetBase.create();
    var _$Container = $('.BaoCaoChamDiemContainer');

    var initBaoCaoChamDiem = function (data) {
        var labels = data.map(p => p.name);

        var chartData = {
            labels: labels,
            datasets: [{
                label: 'Điểm Tự Đánh Giá',
                backgroundColor: '#ffee9e',
                data: data.map(p => p.diemTuDanhGia),
                barPercentage: 0.8,
            }, {
                label: 'Điểm Hội Đồng Thẩm Định',
                backgroundColor: '#ffb3a1',
                data: data.map(p => p.diemHoiDongThamDinh),
                barPercentage: 0.8,
            }]
        }

        var canvas = $(_$Container[0]).find('#bao-cao-cham-diem')
        canvas.css('min-height', data.length*30+'px')

        var myChart = new Chart(canvas, {
            type: 'horizontalBar',
            data: chartData,
            options: {
                //title: {
                //    display: true,
                //    text: "Báo cáo chấm điểm"
                //},
                tooltips: {
                    intersect: false,
                    mode: 'nearest',
                    xPadding: 10,
                    yPadding: 10,
                    caretPadding: 10
                },
                legend: {
                    display: true,
                    position: 'top',
                    //labels: {
                    //    fontColor: 'blue'
                    //}
                },
                responsive: true,
                //maintainAspectRatio: false,
                //barRadius: 4,
                //barThickness: 'flex',
                gridLines: {
                    offsetGridLines: false
                },
                scales: {
                    xAxes: [{
                        //display: false,
                        //gridLines: true,
                        //stacked: true,
                        position: 'top',
                    }],
                    yAxes: [{
                        //display: true,
                        //gridLines: true,
                        //stacked: true,
                        ticks: {
                            beginAtZero: true,
                            fontColor: 'blue',
                        },
                    }]
                },
                layout: {
                    padding: {
                        left: 0,
                        right: 0,
                        top: 0,
                        bottom: 0
                    }
                },
            }
        });
    }

    var getBaoCao = function () {
        abp.ui.setBusy(_$Container);

        _tenantDashboardService
            .getBaoCaoChamDiem()
            .done(function (result) {
                console.log(result);
                initBaoCaoChamDiem(result)
            }).always(function () {
                abp.ui.clearBusy(_$Container);
            });
    };

    _widgetBase.runDelayed(function () {
        getBaoCao();
    });
});