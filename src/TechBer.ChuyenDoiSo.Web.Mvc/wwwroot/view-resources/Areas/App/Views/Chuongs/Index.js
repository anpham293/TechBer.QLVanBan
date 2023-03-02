(function () {
    $(function () {

        var _$chuongsTable = $('#ChuongsTable');
        var _chuongsService = abp.services.app.chuongs;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyDanhMuc.Chuong';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Chuongs.Create'),
            edit: abp.auth.hasPermission('Pages.Chuongs.Edit'),
            'delete': abp.auth.hasPermission('Pages.Chuongs.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Chuongs/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Chuongs/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditChuongModal'
        });       

		 var _viewChuongModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Chuongs/ViewchuongModal',
            modalClass: 'ViewChuongModal'
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

        var dataTable = _$chuongsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chuongsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ChuongsTableFilter').val(),
					maSoFilter: $('#MaSoFilterId').val(),
					tenFilter: $('#TenFilterId').val(),
					capQuanLyTenFilter: $('#CapQuanLyTenFilterId').val()
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
                                    _viewChuongModal.open({ id: data.record.chuong.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.chuong.id });                                
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
                                    entityId: data.record.chuong.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteChuong(data.record.chuong);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "chuong.maSo",
						 name: "maSo"   
					},
					{
						targets: 2,
						 data: "chuong.ten",
						 name: "ten"   
					},
					{
						targets: 3,
						 data: "capQuanLyTen" ,
						 name: "capQuanLyFk.ten" 
					}
            ]
        });

        function getChuongs() {
            dataTable.ajax.reload();
        }

        function deleteChuong(chuong) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _chuongsService.delete({
                            id: chuong.id
                        }).done(function () {
                            getChuongs(true);
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

        $('#CreateNewChuongButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _chuongsService
                .getChuongsToExcel({
				filter : $('#ChuongsTableFilter').val(),
					maSoFilter: $('#MaSoFilterId').val(),
					tenFilter: $('#TenFilterId').val(),
					capQuanLyTenFilter: $('#CapQuanLyTenFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditChuongModalSaved', function () {
            getChuongs();
        });

		$('#GetChuongsButton').click(function (e) {
            e.preventDefault();
            getChuongs();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getChuongs();
		  }
		});
    });
})();