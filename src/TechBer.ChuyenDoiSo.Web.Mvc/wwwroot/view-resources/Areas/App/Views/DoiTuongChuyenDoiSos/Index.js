(function () {
    $(function () {
        const LOAI_PHU_LUC = [
            "None",
            "PHU_LUC_IA",
            "PHU_LUC_IIA",
        ];

        const TRANG_THAI_CHAM_DIEM = [
            {text: "Tự Đánh Giá", class: "badge-primary"},
            {text: "Hội Đồng Thẩm Định", class: "badge-success"},
        ];

        var _$doiTuongChuyenDoiSosTable = $('#DoiTuongChuyenDoiSosTable');
        var _doiTuongChuyenDoiSosService = abp.services.app.doiTuongChuyenDoiSos;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.DoiTuongChuyenDoiSo';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DoiTuongChuyenDoiSos.Create'),
            edit: abp.auth.hasPermission('Pages.DoiTuongChuyenDoiSos.Edit'),
            'delete': abp.auth.hasPermission('Pages.DoiTuongChuyenDoiSos.Delete'),
            chamDiem: abp.auth.hasPermission('Pages.DoiTuongChuyenDoiSos.ChamDiem'),
            hoiDongThamDinh: abp.auth.hasPermission('Pages.DoiTuongChuyenDoiSos.HoiDongThamDinh')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DoiTuongChuyenDoiSos/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DoiTuongChuyenDoiSos/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDoiTuongChuyenDoiSoModal'
        });

		//var _viewDoiTuongChuyenDoiSoModal = new app.ModalManager({
  //          viewUrl: abp.appPath + 'App/DoiTuongChuyenDoiSos/ViewdoiTuongChuyenDoiSoModal',
  //          modalClass: 'ViewDoiTuongChuyenDoiSoModal'
  //      });

		var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
            function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        //var _chamDiemModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'App/DoiTuongChuyenDoiSos/ChamDiemdoiTuongChuyenDoiSoModal',
        //    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DoiTuongChuyenDoiSos/_ChamDiemdoiTuongChuyenDoiSoModal.js',
        //    modalClass: 'ChamDiemDoiTuongChuyenDoiSoModal',
        //    //modalSize: 'chieu-rong-modal'
        //});

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() === null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$doiTuongChuyenDoiSosTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _doiTuongChuyenDoiSosService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DoiTuongChuyenDoiSosTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        phuLucFilter: $('#PhuLucFilter').val(),
                        userNameFilter: $('#UserNameFilterId').val()
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
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.doiTuongChuyenDoiSo.id });                                
                                }
                            },
                            {
                                text: app.localize('ChamDiem'),
                                visible: function () {
                                    return (_permissions.chamDiem || _permissions.hoiDongThamDinh);
                                },
                                action: function (data) {
                                    window.open("DoiTuongChuyenDoiSos/TinhDiem?id=" + data.record.doiTuongChuyenDoiSo.id)
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
                                        entityId: data.record.doiTuongChuyenDoiSo.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteDoiTuongChuyenDoiSo(data.record.doiTuongChuyenDoiSo);
                                }
                            },
                            {
                                text: app.localize('ChuyenDoiChamDiemFlag'),
                                visible: function () {
                                    return _permissions.hoiDongThamDinh;
                                },
                                action: function (data) {
                                    _doiTuongChuyenDoiSosService.chuyenTrangThaiChamDiem({ doiTuongId: data.record.doiTuongChuyenDoiSo.id, chamDiemFlag: 1 - data.record.doiTuongChuyenDoiSo.chamDiemFlag }).done(function () {
                                        getDoiTuongChuyenDoiSos();
                                    })
                                }
                            }
                        ]
                    }
                },
				{
					targets: 1,
					data: "doiTuongChuyenDoiSo.name",
					name: "name"   
				},
				{
					targets: 2,
                    render: function (displayName, type, row, meta) {
                        return LOAI_PHU_LUC[row.doiTuongChuyenDoiSo.type]
                    }
				},
				{
					targets: 3,
					data: "userName" ,
					name: "userFk.name" 
                },
                {
                    targets: 4,
                    data: "doiTuongChuyenDoiSo.tongDiemTuDanhGia",
                    name: "tongDiemTuDanhGia"
                },
                {
                    targets: 5,
                    data: "doiTuongChuyenDoiSo.tongDiemHoiDongThamDinh",
                    name: "tongDiemHoiDongThamDinh"
                },
                {
                    targets: 6,
                    data: "doiTuongChuyenDoiSo.tongDiemDatDuoc",
                    name: "tongDiemDatDuoc"
                },
                {
                    targets: 7,
                    render: function (displayName, type, row, meta) {
                        return '<span class="badge ' + TRANG_THAI_CHAM_DIEM[row.doiTuongChuyenDoiSo.chamDiemFlag].class +'">' + TRANG_THAI_CHAM_DIEM[row.doiTuongChuyenDoiSo.chamDiemFlag].text + '</span>'
                    }
                }
            ]
        });

        function getDoiTuongChuyenDoiSos() {
            dataTable.ajax.reload();
        }

        function deleteDoiTuongChuyenDoiSo(doiTuongChuyenDoiSo) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _doiTuongChuyenDoiSosService.delete({
                            id: doiTuongChuyenDoiSo.id
                        }).done(function () {
                            getDoiTuongChuyenDoiSos(true);
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

        $('#CreateNewDoiTuongChuyenDoiSoButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _doiTuongChuyenDoiSosService
                .getDoiTuongChuyenDoiSosToExcel({
				filter : $('#DoiTuongChuyenDoiSosTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					minTypeFilter: $('#MinTypeFilterId').val(),
					maxTypeFilter: $('#MaxTypeFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDoiTuongChuyenDoiSoModalSaved', function () {
            getDoiTuongChuyenDoiSos();
        });

		$('#GetDoiTuongChuyenDoiSosButton').click(function (e) {
            e.preventDefault();
            getDoiTuongChuyenDoiSos();
        });

		$(document).keypress(function(e) {
            if(e.which === 13) {
                getDoiTuongChuyenDoiSos();
            }
		});
    });
})();