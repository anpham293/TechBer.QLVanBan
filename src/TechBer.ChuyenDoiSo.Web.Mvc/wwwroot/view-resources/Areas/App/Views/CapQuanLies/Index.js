(function () {
    $(function () {

        var _$capQuanLiesTable = $('#CapQuanLiesTable');
        var _capQuanLiesService = abp.services.app.capQuanLies;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyDanhMuc.CapQuanLy';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.CapQuanLies.Create'),
            edit: abp.auth.hasPermission('Pages.CapQuanLies.Edit'),
            'delete': abp.auth.hasPermission('Pages.CapQuanLies.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CapQuanLies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CapQuanLies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditCapQuanLyModal'
        });       

		 var _viewCapQuanLyModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CapQuanLies/ViewcapQuanLyModal',
            modalClass: 'ViewCapQuanLyModal'
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

        var dataTable = _$capQuanLiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _capQuanLiesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CapQuanLiesTableFilter').val(),
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
                                    _viewCapQuanLyModal.open({ id: data.record.capQuanLy.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.capQuanLy.id });                                
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
                                    entityId: data.record.capQuanLy.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCapQuanLy(data.record.capQuanLy);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "capQuanLy.ten",
						 name: "ten"   
					}
            ]
        });

        function getCapQuanLies() {
            dataTable.ajax.reload();
        }

        function deleteCapQuanLy(capQuanLy) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _capQuanLiesService.delete({
                            id: capQuanLy.id
                        }).done(function () {
                            getCapQuanLies(true);
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

        $('#CreateNewCapQuanLyButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _capQuanLiesService
                .getCapQuanLiesToExcel({
				filter : $('#CapQuanLiesTableFilter').val(),
					tenFilter: $('#TenFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCapQuanLyModalSaved', function () {
            getCapQuanLies();
        });

		$('#GetCapQuanLiesButton').click(function (e) {
            e.preventDefault();
            getCapQuanLies();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCapQuanLies();
		  }
		});
    });
})();