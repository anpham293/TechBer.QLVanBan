(function () {
    $(function () {
        $(document).ready(function () {
            function fnExcelReport(tableid) {

                var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
                var textRange;
                var j = 0;
                tab = document.getElementById(tableid); // id of table

                for (j = 0; j < tab.rows.length; j++) {
                    tab_text = tab_text + tab.rows[j].innerHTML.replace(",", "") + "</tr>";
                    //tab_text=tab_text+"</tr>";
                }

                tab_text = tab_text + "</table>";
                tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
                tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
                tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

                var ua = window.navigator.userAgent;
                var msie = ua.indexOf("MSIE ");

                if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
                {
                    txtArea1.document.open("txt/html", "replace");
                    txtArea1.document.write(tab_text);
                    txtArea1.document.close();
                    txtArea1.focus();
                    sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xlsx");
                } else                 //other browser not tested on IE 11
                    sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text.replace(",", "")));

                return (sa);
            }

            $(document).on('click', '#btn-xuatexcel', function () {

                fnExcelReport("table-r");
            });

            $("#btn-xuatbc").on('click', function () {
                $.ajax({
                    url: abp.appPath + 'App/QuyTrinhDuAnAssigneds/BaoCaoHoSoTheoDuAn',
                    type: "post",
                    dataType: "html",
                    data: {
                        maDuAn: $("#MaDuAn").val(),
                        tenDuAn: $("#TenDuAn").val(),
                        tenHoSo: $("#TenHoSo").val(),
                        ngayQuyetDinh: $("#NgayQuyetDinh").val()
                    },
                    beforeSend: function () {
                        $("#table-result").html("");
                        abp.ui.setBusy("body");
                    },
                    success: function (data) {

                        $("#table-result").html(data);
                    },
                    complete: function () {
                        abp.ui.clearBusy("body")
                    }
                })
            });
        })
    });
})();
