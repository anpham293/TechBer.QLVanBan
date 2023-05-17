(function () {
    $(function () {

        var _$thungHoSosTable = $('#ThungHoSosTable');
        var _thungHoSosService = abp.services.app.thungHoSos;
        var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyKhoHoSo.ThungHoSo';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ThungHoSos.Create'),
            edit: abp.auth.hasPermission('Pages.ThungHoSos.Edit'),
            'delete': abp.auth.hasPermission('Pages.ThungHoSos.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ThungHoSos/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ThungHoSos/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditThungHoSoModal',
            modalSize: 'modal-xl'
        });

        var _viewThungHoSoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ThungHoSos/ViewthungHoSoModal',
            modalClass: 'ViewThungHoSoModal'
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        };

        function isEmptyOrSpaces(str) {
            return str === null || str.match(/^ *$/) !== null;
        }

        var _qroptions = {
            text: "",
            width: 300,
            height: 300,
            logo: "/Common/Images/techber.png",
            quietZone: 15,
            quietZoneColor: "rgba(247,247,247,62%)",
        };

        var dataTable = _$thungHoSosTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _thungHoSosService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#ThungHoSosTableFilter').val(),
                        maSoFilter: $('#MaSoFilterId').val(),
                        tenFilter: $('#TenFilterId').val(),
                        moTaFilter: $('#MoTaFilterId').val(),
                        minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
                        maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val(),
                        dayKeMaSoFilter: $('#DayKeMaSoFilterId').val(),
                        duAnNameFilter: $('#DuAnNameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
                            {
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewThungHoSoModal.open({id: data.record.thungHoSo.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.thungHoSo.id});
                                }
                            },
                            {
                                text: app.localize('mo'),
                                action: function (data) {
                                    window.open("https://localhost:6666");
                                }
                            },
                            {
                                text: app.localize('History'),
                                visible: function () {
                                    return entityHistoryIsEnabled();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.thungHoSo.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteThungHoSo(data.record.thungHoSo);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "thungHoSo.maSo",
                    name: "maSo"
                },
                {
                    targets: 2,
                    data: "thungHoSo.ten",
                    name: "ten"
                },
                {
                    targets: 3,
                    data: "thungHoSo.moTa",
                    name: "moTa"
                },
                {
                    targets: 4,
                    data: "thungHoSo.trangThai",
                    name: "trangThai"
                },
                {
                    targets: 5,
                    data: "dayKeMaSo",
                    name: "dayKeFk.maSo"
                },
                {
                    targets: 6,
                    width: 30,
                    data: "thungHoSo.qrString",
                    name: "qrstring",
                    render: function (qrstring) {
                        return "<a class='qrstring-click' style='cursor: pointer' data-target='" + qrstring + "'><i class='fa fa-qrcode' style='font-size: xx-large'></i></a>";
                    }
                },
                {
                    targets: 7,
                    data: "duAnName",
                    name: "duAnFk.name"
                }
            ]
        });

        function getThungHoSos() {
            dataTable.ajax.reload();
        }

        function deleteThungHoSo(thungHoSo) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _thungHoSosService.delete({
                            id: thungHoSo.id
                        }).done(function () {
                            getThungHoSos(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }


        function printPage(e, f, title) {
            var w = window.open();
            var field = $("#" + e).html();
            var html = "<!DOCTYPE HTML>";
            html += '<html lang="en-us">';
            html += '<head><title>' + title + '</title></head>';

            html += "<body style='padding: 0px 0px 0px 0px!important'>" + f;

            //check to see if they are null so "undefined" doesnt print on the page. <br>s optional, just to give space

            if (field != null) html += field;

            html += "</body>";
            setTimeout(w.document.write(html),1000);
            setTimeout(w.window.print(), 1000);
            w.document.close();
        }
        
        $(document).on("click", ".qrstring-click", function () {
            var QRContainer = document.createElement("div");
            QRContainer.className = "text-center";
            QRContainer.id = "qrcontainer";
            QRContainer.textContent = "";
            result = $(this).attr("data-target");
            if (!isEmptyOrSpaces(result)) {
                _qroptions.text = "https://truyxuatnguongoc.techber.vn/Truyxuat?id="+result;
                var qrCode = new QRCode(QRContainer, _qroptions);
                //TODO -- In QR code
                console.log(qrCode);
                console.log(qrCode._oDrawing);
                console.log(qrCode._oDrawing.dataURL);
                QRContainer.title = qrCode._oDrawing.dataURL;
                console.log(QRContainer);
                sweetAlert({
                    content: QRContainer,
                    title: "QRCode Thùng hồ sơ",
                    buttons: [
                        {
                            visible: true,
                            text: "In QRCode",
                            value: 3,
                            className: "swal-button inqrcode",
                            closeModal: false,
                        },
                        {
                            visible: true,
                            text: "Đóng",
                            value: 1,
                            className: "",
                            closeModal: true,
                        }
                    ]
                }).then(function (result) {
                   if(result === 3){
                       var htmlQrCode = '<img alt="qrcode" src="'+ qrCode._oDrawing.dataURL+'"/>';
                       printPage("afsafasf", htmlQrCode, "QR thùng hồ sơ",);
                   }
                })
            } else {
                sweetAlert({
                    text: app.localize("CoLoiXayRa")
                })
            }
        });

        
        
        // $(document).on("click",".inqrcode",function () {
        //    
        //     // printPage("afsafasf",htmlResult , "QR thùng hồ sơ");
        // });

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewThungHoSoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _thungHoSosService
                .getThungHoSosToExcel({
                    filter: $('#ThungHoSosTableFilter').val(),
                    maSoFilter: $('#MaSoFilterId').val(),
                    tenFilter: $('#TenFilterId').val(),
                    moTaFilter: $('#MoTaFilterId').val(),
                    minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
                    maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val(),
                    dayKeMaSoFilter: $('#DayKeMaSoFilterId').val(),
                    duAnNameFilter: $('#DuAnNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditThungHoSoModalSaved', function () {
            getThungHoSos();
        });

        $('#GetThungHoSosButton').click(function (e) {
            e.preventDefault();
            getThungHoSos();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getThungHoSos();
            }
        });
    });
})();