(function () {
    $(function () {

        var _$phongKhosTable = $('#PhongKhosTable');
        var _phongKhosService = abp.services.app.phongKhos;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyKhoHoSo.PhongKho';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PhongKhos.Create'),
            edit: abp.auth.hasPermission('Pages.PhongKhos.Edit'),
            'delete': abp.auth.hasPermission('Pages.PhongKhos.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PhongKhos/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PhongKhos/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPhongKhoModal'
        });       

		 var _viewPhongKhoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PhongKhos/ViewphongKhoModal',
            modalClass: 'ViewPhongKhoModal'
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

        var dataTable = _$phongKhosTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _phongKhosService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PhongKhosTableFilter').val(),
					maSoFilter: $('#MaSoFilterId').val(),
					tenFilter: $('#TenFilterId').val()
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
                                    _viewPhongKhoModal.open({ id: data.record.phongKho.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.phongKho.id });                                
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
                                    entityId: data.record.phongKho.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePhongKho(data.record.phongKho);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "phongKho.maSo",
						 name: "maSo"   
					},
					{
						targets: 2,
						 data: "phongKho.ten",
						 name: "ten"   
					}
            ]
        });

        function getPhongKhos() {
            dataTable.ajax.reload();
        }

        function deletePhongKho(phongKho) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _phongKhosService.delete({
                            id: phongKho.id
                        }).done(function () {
                            getPhongKhos(true);
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

        $('#CreateNewPhongKhoButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _phongKhosService
                .getPhongKhosToExcel({
				filter : $('#PhongKhosTableFilter').val(),
					maSoFilter: $('#MaSoFilterId').val(),
					tenFilter: $('#TenFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPhongKhoModalSaved', function () {
            getPhongKhos();
        });

		$('#GetPhongKhosButton').click(function (e) {
            e.preventDefault();
            getPhongKhos();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPhongKhos();
		  }
		});
    });
})();