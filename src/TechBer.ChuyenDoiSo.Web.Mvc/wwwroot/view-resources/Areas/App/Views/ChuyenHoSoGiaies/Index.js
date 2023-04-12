(function () {
    $(function () {

        var _$chuyenHoSoGiaiesTable = $('#ChuyenHoSoGiaiesTable');
        var _chuyenHoSoGiaiesService = abp.services.app.chuyenHoSoGiaies;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.ChuyenHoSoGiay';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ChuyenHoSoGiaies.Create'),
            edit: abp.auth.hasPermission('Pages.ChuyenHoSoGiaies.Edit'),
            'delete': abp.auth.hasPermission('Pages.ChuyenHoSoGiaies.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChuyenHoSoGiaies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChuyenHoSoGiaies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditChuyenHoSoGiayModal'
        });       

		 var _viewChuyenHoSoGiayModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChuyenHoSoGiaies/ViewchuyenHoSoGiayModal',
            modalClass: 'ViewChuyenHoSoGiayModal'
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

        var dataTable = _$chuyenHoSoGiaiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chuyenHoSoGiaiesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#ChuyenHoSoGiaiesTableFilter').val(),
                        vanBanDuAnId: $('#vanBanDuAnId').val()
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
                                    _viewChuyenHoSoGiayModal.open({ id: data.record.chuyenHoSoGiay.id });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return data.record.chuyenHoSoGiay.trangThai == 0;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.chuyenHoSoGiay.id });
                                }
                            }
                        ]
                    }
                },
                {
                    targets: 1,
                    data: "vanBanDuAnName",
                    name: "vanBanDuAnName"
                },
                {
                    targets: 2,
                    data: "tenNguoiChuyen",
                    name: "nguoiChuyenId"
                },
                {
                    targets: 3,
                    data: "chuyenHoSoGiay.thoiGianChuyen",
                    name: "thoiGianChuyen" ,
                    render: function (thoiGianChuyen) {
                        if (thoiGianChuyen) {
                            return moment(thoiGianChuyen).format('DD/MM/YYYY hh:MM:ss');
                        }
                        return "";
                    }

                },
                {
                    targets: 4,
                    data: "chuyenHoSoGiay.soLuong",
                    name: "soLuong"
                },
                {
                    targets: 5,
                    data: "chuyenHoSoGiay.trangThai",
                    name: "trangThai",
                    className: "text-center",
                    render: function (data) {
                        var trangThai = "";
                        if(data == app.trangThaiChuyenHoSoGiay.chuaGuiHoSo){
                            trangThai = "<label class='badge badge-danger'>Chưa gửi hồ sơ</label>";
                        }
                        if(data == app.trangThaiChuyenHoSoGiay.dangChoTiepNhan){
                            trangThai = "<label class='badge badge-primary'>Chờ tiếp nhận</label>";
                        }
                        if(data == app.trangThaiChuyenHoSoGiay.daNhanHoSo){
                            trangThai = "<label class='badge badge-success'>Đã nhận hồ sơ</label>";
                        }
                        return trangThai;
                    }
                },
                {
                    targets: 6,
                    data: "chuyenHoSoGiay.thoiGianNhan",
                    name: "thoiGianNhan" ,
                    render: function (thoiGianNhan) {
                        if (thoiGianNhan) {
                            return moment(thoiGianNhan).format('DD/MM/YYYY hh:MM:ss');
                        }
                        return "";
                    }
                },
                {
                    targets: 7,
                    data: "tenNguoiNhan" ,
                    name: "nguoiNhanFk.name"
                },
                {
                    targets: 8,

                    data: "chuyenHoSoGiay",
                    name: "trangThai",
                    className: "text-center",
                    render: function(chuyenHoSoGiay, type, row, meta){
                        console.log(row);
                        console.log(chuyenHoSoGiay);
                        var guiHoSo = "";
                        var nhanHoSo = "";
                        if(chuyenHoSoGiay.trangThai == app.trangThaiChuyenHoSoGiay.chuaGuiHoSo && row.userId == chuyenHoSoGiay.nguoiChuyenId){
                            guiHoSo = "<a class='text-info guiHoSo' chuyenHoSoGiayId='"+ chuyenHoSoGiay.id +"' style='cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;'><i class=\"fas fa-paper-plane\"></i></a>";
                        }
                        if(chuyenHoSoGiay.trangThai == app.trangThaiChuyenHoSoGiay.dangChoTiepNhan  && row.userId == chuyenHoSoGiay.nguoiNhanId)
                        {
                            nhanHoSo = "<a class='text-success nhanHoSo' chuyenHoSoGiayId='"+ chuyenHoSoGiay.id +"' style='cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;'><i class=\"fas fa-hands-helping\"></i></a>";
                        }
                        return guiHoSo + " " + nhanHoSo;
                    }
                },
            ]
        });

        $(document).on('click', '.guiHoSo', function () {
            var self = $(this);
            var id = self.attr('chuyenHoSoGiayId');
            //trang thai old = chua gui, mew = cho tiep nhan
            var trangThai = app.trangThaiChuyenHoSoGiay.dangChoTiepNhan;
            abp.ui.setBusy($("body"));

            abp.message.confirm(
                app.localize("GuiHoSo") + "?",
                app.localize("AreYouSure"),
                function (isConfirmed) {
                    if (isConfirmed){
                        _chuyenHoSoGiaiesService.capNhatTrangThaiChuyenHoSoGiay(id, trangThai).done(function () {
                            getChuyenHoSoGiaies();
                            abp.ui.clearBusy($("body"));
                        });
                    }
                    abp.ui.clearBusy($("body"));
                }
            )
        });

        $(document).on('click', '.nhanHoSo', function () {
            var self = $(this);
            var id = self.attr('chuyenHoSoGiayId');
            //trang thai old = chua gui, mew = cho tiep nhan
            var trangThai = app.trangThaiChuyenHoSoGiay.daNhanHoSo;
            abp.ui.setBusy($("body"));

            abp.message.confirm(
                app.localize("NhanHoSo") + "?",
                app.localize("AreYouSure"),
                function (isConfirmed) {
                    if (isConfirmed){
                        _chuyenHoSoGiaiesService.capNhatTrangThaiChuyenHoSoGiay(id, trangThai).done(function () {
                            getChuyenHoSoGiaies();
                            abp.ui.clearBusy($("body"));
                        });
                    }
                    abp.ui.clearBusy($("body"));
                }
            )
        });

        function getChuyenHoSoGiaies() {
            dataTable.ajax.reload();
        }

        function deleteChuyenHoSoGiay(chuyenHoSoGiay) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _chuyenHoSoGiaiesService.delete({
                            id: chuyenHoSoGiay.id
                        }).done(function () {
                            getChuyenHoSoGiaies(true);
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

        $('#CreateNewChuyenHoSoGiayButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _chuyenHoSoGiaiesService
                .getChuyenHoSoGiaiesToExcel({
				filter : $('#ChuyenHoSoGiaiesTableFilter').val(),
					minNguoiChuyenIdFilter: $('#MinNguoiChuyenIdFilterId').val(),
					maxNguoiChuyenIdFilter: $('#MaxNguoiChuyenIdFilterId').val(),
					minThoiGianChuyenFilter:  getDateFilter($('#MinThoiGianChuyenFilterId')),
					maxThoiGianChuyenFilter:  getDateFilter($('#MaxThoiGianChuyenFilterId')),
					minSoLuongFilter: $('#MinSoLuongFilterId').val(),
					maxSoLuongFilter: $('#MaxSoLuongFilterId').val(),
					minTrangThaiFilter: $('#MinTrangThaiFilterId').val(),
					maxTrangThaiFilter: $('#MaxTrangThaiFilterId').val(),
					minThoiGianNhanFilter:  getDateFilter($('#MinThoiGianNhanFilterId')),
					maxThoiGianNhanFilter:  getDateFilter($('#MaxThoiGianNhanFilterId')),
					vanBanDuAnNameFilter: $('#VanBanDuAnNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditChuyenHoSoGiayModalSaved', function () {
            getChuyenHoSoGiaies();
        });

		$('#GetChuyenHoSoGiaiesButton').click(function (e) {
            e.preventDefault();
            getChuyenHoSoGiaies();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getChuyenHoSoGiaies();
		  }
		});
    });
})();