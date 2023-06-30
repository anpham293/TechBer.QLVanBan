(function () {
    $(function () {

        var _$quyetDinhsTable = $('#QuyetDinhsTable');
        var _quyetDinhsService = abp.services.app.quyetDinhs;
        var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.QuyetDinh';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.QuyetDinhs.Create'),
            edit: abp.auth.hasPermission('Pages.QuyetDinhs.Edit'),
            'delete': abp.auth.hasPermission('Pages.QuyetDinhs.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyetDinhs/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyetDinhs/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditQuyetDinhModal'
        });
        var _viewQuyetDinhFileDoc = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyetDinhs/ViewQuyetDinhFileDoc',
            modalSize: "modal-xl"
        });
        var _viewQuyetDinhFilePdf = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyetDinhs/ViewQuyetDinhFilePdf',
            modalClass: 'ViewQuyetDinhModal',
            modalSize: "modal-xl"
        });

        var _viewQuyetDinhModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyetDinhs/ViewquyetDinhModal',
            modalClass: 'ViewQuyetDinhModal'
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
        }

        var dataTable = _$quyetDinhsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _quyetDinhsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#QuyetDinhsTableFilter').val(),
                        soFilter: $('#SoFilterId').val(),
                        tenFilter: $('#TenFilterId').val(),
                        minNgayBanHanhFilter: getDateFilter($('#MinNgayBanHanhFilterId')),
                        maxNgayBanHanhFilter: getDateFilter($('#MaxNgayBanHanhFilterId')),
                        fileQuyetDinhFilter: $('#FileQuyetDinhFilterId').val(),
                        minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
                        maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val()
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
                                    _viewQuyetDinhModal.open({id: data.record.quyetDinh.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.quyetDinh.id});
                                }
                            },
                            // {
                            //     text: app.localize('XEM'),
                            //     action: function (data) {
                            //         var a = data.record.quyetDinh.fileQuyetDinh;
                            //         var b = JSON.parse(a);
                            //         var c = b.ContentType;
                            //         if(c=='application/pdf'){
                            //             _viewQuyetDinhFilePdf.open({id: data.record.quyetDinh.id});
                            //         }
                            //         else{
                            //             _viewQuyetDinhFileDoc.open({id: data.record.quyetDinh.id});
                            //         } 
                            //        
                            //      
                            //     }
                            // },
                            {
                                text: app.localize('History'),
                                visible: function () {
                                    return entityHistoryIsEnabled();
                                },
                                action: function (data) {
                                    _entityTypeHistoryModal.open({
                                        entityTypeFullName: _entityTypeFullName,
                                        entityId: data.record.quyetDinh.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteQuyetDinh(data.record.quyetDinh);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "quyetDinh.so",
                    name: "so"
                },
                {
                    targets: 2,
                    data: "quyetDinh.ten",
                    name: "ten"
                },
                {
                    targets: 3,
                    data: "quyetDinh.ngayBanHanh",
                    name: "ngayBanHanh",
                    render: function (ngayBanHanh) {
                        if (ngayBanHanh) {
                            return moment(ngayBanHanh).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 4,
                    data: "quyetDinh.fileQuyetDinh",
                    name: "fileQuyetDinh",
                    className: "text-center",
                    render: function (data, type, row, meta) {
                        var file = row.quyetDinh.fileQuyetDinh;
                        if(file == null || file == ''){
                            return '';
                        }else {
                            var jsonFile = JSON.parse(file);
                            var contentTypeJsonFile = jsonFile.ContentType;
                            if(contentTypeJsonFile == 'application/pdf'){
                                return  "<a class='text-warning text-bold view-pdf' data-target='" + row.quyetDinh.id + "' style='cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;'><i class=\"fas fa-file-pdf\"></i></a>";
                            }
                            else {
                                return  "<a class='text-info text-bold view-word' data-target='" + row.quyetDinh.id + "' style='cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;'><i class=\"fas fa-file-word\"></i></a>";
                            }
                        }
                       
                    }
                }
            ]
        });

        $(document).on("click", ".view-pdf", function () {
            var self = $(this);
            _viewQuyetDinhFilePdf.open({id: self.attr("data-target")});
        });

        $(document).on("click", ".view-word", function () {
            var self = $(this);
            _viewQuyetDinhFileDoc.open({id: self.attr("data-target")});
        });
        
        function getQuyetDinhs() {
            dataTable.ajax.reload();
        }

        function deleteQuyetDinh(quyetDinh) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _quyetDinhsService.delete({
                            id: quyetDinh.id
                        }).done(function () {
                            getQuyetDinhs(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

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

        $('#CreateNewQuyetDinhButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _quyetDinhsService
                .getQuyetDinhsToExcel({
                    filter: $('#QuyetDinhsTableFilter').val(),
                    soFilter: $('#SoFilterId').val(),
                    tenFilter: $('#TenFilterId').val(),
                    minNgayBanHanhFilter: getDateFilter($('#MinNgayBanHanhFilterId')),
                    maxNgayBanHanhFilter: getDateFilter($('#MaxNgayBanHanhFilterId')),
                    fileQuyetDinhFilter: $('#FileQuyetDinhFilterId').val(),
                    minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
                    maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditQuyetDinhModalSaved', function () {
            getQuyetDinhs();
        });

        $('#GetQuyetDinhsButton').click(function (e) {
            e.preventDefault();
            getQuyetDinhs();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getQuyetDinhs();
            }
        });
    });
})();