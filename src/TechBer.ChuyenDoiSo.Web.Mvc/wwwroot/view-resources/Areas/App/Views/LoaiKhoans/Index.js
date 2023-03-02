(function () {
    $(function () {

        var _$loaiKhoansTable = $('#LoaiKhoansTable');
        var _loaiKhoansService = abp.services.app.loaiKhoans;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyDanhMuc.LoaiKhoan';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.LoaiKhoans.Create'),
            edit: abp.auth.hasPermission('Pages.LoaiKhoans.Edit'),
            'delete': abp.auth.hasPermission('Pages.LoaiKhoans.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LoaiKhoans/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LoaiKhoans/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLoaiKhoanModal'
        });       

		 var _viewLoaiKhoanModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LoaiKhoans/ViewloaiKhoanModal',
            modalClass: 'ViewLoaiKhoanModal'
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

        var dataTable = _$loaiKhoansTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _loaiKhoansService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#LoaiKhoansTableFilter').val(),
					maSoFilter: $('#MaSoFilterId').val(),
					tenFilter: $('#TenFilterId').val(),
					ghiChuFilter: $('#GhiChuFilterId').val()
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
                                    _viewLoaiKhoanModal.open({ id: data.record.loaiKhoan.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.loaiKhoan.id });                                
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
                                    entityId: data.record.loaiKhoan.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteLoaiKhoan(data.record.loaiKhoan);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "loaiKhoan.maSo",
						 name: "maSo"   
					},
					{
						targets: 2,
						 data: "loaiKhoan.ten",
						 name: "ten"   
					},
					{
						targets: 3,
						 data: "loaiKhoan.ghiChu",
						 name: "ghiChu"   
					}
            ]
        });

        function getLoaiKhoans() {
            dataTable.ajax.reload();
        }

        function deleteLoaiKhoan(loaiKhoan) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _loaiKhoansService.delete({
                            id: loaiKhoan.id
                        }).done(function () {
                            getLoaiKhoans(true);
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

        $('#CreateNewLoaiKhoanButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _loaiKhoansService
                .getLoaiKhoansToExcel({
				filter : $('#LoaiKhoansTableFilter').val(),
					maSoFilter: $('#MaSoFilterId').val(),
					tenFilter: $('#TenFilterId').val(),
					ghiChuFilter: $('#GhiChuFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditLoaiKhoanModalSaved', function () {
            getLoaiKhoans();
        });

		$('#GetLoaiKhoansButton').click(function (e) {
            e.preventDefault();
            getLoaiKhoans();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getLoaiKhoans();
		  }
		});
    });
})();