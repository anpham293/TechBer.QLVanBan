(function () {
    $(function () {

        var _$quyTrinhDuAnAssignedsTable = $('#QuyTrinhDuAnAssignedsTable');
        var _quyTrinhDuAnAssignedsService = abp.services.app.quyTrinhDuAnAssigneds;
        var _vanBanDuAns = abp.services.app.vanBanDuAns;
        var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.QuyTrinhDuAnAssigned';

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.QuyTrinhDuAnAssigneds.Create'),
            edit: abp.auth.hasPermission('Pages.QuyTrinhDuAnAssigneds.Edit'),
            'delete': abp.auth.hasPermission('Pages.QuyTrinhDuAnAssigneds.Delete')
        };

        var _chuyenDuyetHoSoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAnAssigneds/ChuyenDuyetHoSoModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAnAssigneds/_ChuyenDuyetHoSoModal.js',
            modalClass: 'ChuyenDuyetHoSoModal'
        });
        
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAnAssigneds/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAnAssigneds/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditQuyTrinhDuAnAssignedModal'
        });

        var _viewQuyTrinhDuAnAssignedModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAnAssigneds/ViewquyTrinhDuAnAssignedModal',
            modalClass: 'ViewQuyTrinhDuAnAssignedModal'
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

        var _viewVanBanDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/VanBanDuAns/ViewvanBanDuAnModal',
            modalClass: 'ViewVanBanDuAnModal',
            modalSize: "modal-xl"
        });
        var dataTable = _$quyTrinhDuAnAssignedsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _vanBanDuAns.getAllHoSoCanDuyet,
                inputFilter: function () {
                    return {
                        textFilter: $('#TextFilter').val(),
                        duAnNameFilter: $('#DuAnNameFilter').val(),
                        trangThaiDuyetFilter: $('#TrangThaiDuyetFilter').val()
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
                                text: app.localize('DuyetHoSo'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _chuyenDuyetHoSoModal.open({
                                        vanBanDuAnId: data.record.vanBanDuAn.id,
                                        typeDuyetHoSo : app.typeDuyetHoSoConst.chanhVanPhongDuyet
                                        // type = 1: quản lý, 2: chánh vp
                                    });
                                }
                            },
                            {
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewVanBanDuAnModal.open({id: data.record.vanBanDuAn.id});
                                }
                            },
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "vanBanDuAn",
                    name: "name",
                    render(vanBanDuAn){
                        return '<a style="white-space: normal">'+ vanBanDuAn.name +'</a>';
                    }
                },
                {
                    targets: 2,
                    data: "vanBanDuAn",
                    name: "name",
                    render(vanBanDuAn){
                        return '<a style="white-space: normal">'+ vanBanDuAn.soLuongVanBanGiay +'</a>';
                    }
                },
                {
                    targets: 3,
                    data: "vanBanDuAn.ngayGui",
                    name: "thoiGianNhan" ,
                    render: function (thoiGianNhan) {
                        if (thoiGianNhan) {
                            return moment(thoiGianNhan).format('L');
                        }
                        return "";
                    }
                },
                {
                    targets: 4,
                    data: "vanBanDuAn.soLuongVanBanGiay",
                    name: "soLuong"
                },
                {
                    targets: 5,
                    data: "vanBanDuAn",
                    name: "name",
                    className: "text-center",
                    render(displayName, type, row, meta){
                        if(row.vanBanDuAn.trangThaiChuyenDuyetHoSo == app.trangThaiDuyetHoSoConst.dangChoDuyet){
                            return "<labe class='badge badge-info'>Đang chờ duyệt <br>T.gian gửi: "+ moment(row.vanBanDuAn.ngayGui).format('DD/MM/YYYY HH:mm:ss') +"</label>"
                        }
                        if(row.vanBanDuAn.trangThaiChuyenDuyetHoSo == app.trangThaiDuyetHoSoConst.daDuyet){
                            return "<label class='badge badge-success'>Đã duyệt <br>T.gian duyệt: "+ moment(row.vanBanDuAn.ngayDuyet).format('DD/MM/YYYY HH:mm:ss') +" </label>"
                        }
                    }
                },
                {
                    targets: 6,
                    data: "vanBanDuAn.ngayGui",
                    name: "thoiGianNhan" ,
                    render: function (thoiGianNhan) {
                        if (thoiGianNhan) {
                            return moment(thoiGianNhan).format('L');
                        }
                        return "";
                    }
                },
                {
                    targets: 7,
                    data: "vanBanDuAn.name" ,
                    name: "nguoiNhanFk.name"
                },
            ]
        });

        function getQuyTrinhDuAnAssigneds() {
            dataTable.ajax.reload();
        }

        function deleteQuyTrinhDuAnAssigned(quyTrinhDuAnAssigned) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _quyTrinhDuAnAssignedsService.delete({
                            id: quyTrinhDuAnAssigned.id
                        }).done(function () {
                            getQuyTrinhDuAnAssigneds(true);
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

        $('#CreateNewQuyTrinhDuAnAssignedButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _quyTrinhDuAnAssignedsService
                .getQuyTrinhDuAnAssignedsToExcel({
                    filter : $('#QuyTrinhDuAnAssignedsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionsFilter: $('#DescriptionsFilterId').val(),
                    minSTTFilter: $('#MinSTTFilterId').val(),
                    maxSTTFilter: $('#MaxSTTFilterId').val(),
                    minSoVanBanQuyDinhFilter: $('#MinSoVanBanQuyDinhFilterId').val(),
                    maxSoVanBanQuyDinhFilter: $('#MaxSoVanBanQuyDinhFilterId').val(),
                    maQuyTrinhFilter: $('#MaQuyTrinhFilterId').val(),
                    loaiDuAnNameFilter: $('#LoaiDuAnNameFilterId').val(),
                    quyTrinhDuAnNameFilter: $('#QuyTrinhDuAnNameFilterId').val(),
                    quyTrinhDuAnAssignedNameFilter: $('#QuyTrinhDuAnAssignedNameFilterId').val(),
                    duAnNameFilter: $('#DuAnNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditVanBanDuAnModalSaved', function () {
            getQuyTrinhDuAnAssigneds();
        });

        $('#GetQuyTrinhDuAnAssignedsButton').click(function (e) {
            e.preventDefault();
            getQuyTrinhDuAnAssigneds();
        });

        $(document).keypress(function(e) {
            if(e.which === 13) {
                getQuyTrinhDuAnAssigneds();
            }
        });
    });
})();