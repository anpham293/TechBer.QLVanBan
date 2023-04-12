(function () {
    $(function () {

        var _$dayKesTable = $('#DayKesTable');
        var _dayKesService = abp.services.app.dayKes;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyKhoHoSo.DayKe';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DayKes.Create'),
            edit: abp.auth.hasPermission('Pages.DayKes.Edit'),
            'delete': abp.auth.hasPermission('Pages.DayKes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DayKes/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DayKes/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDayKeModal'
        });       

		 var _viewDayKeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DayKes/ViewdayKeModal',
            modalClass: 'ViewDayKeModal'
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

        var dataTable = _$dayKesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _dayKesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#DayKesTableFilter').val(),
					maSoFilter: $('#MaSoFilterId').val(),
					tenFilter: $('#TenFilterId').val(),
					phongKhoMaSoFilter: $('#PhongKhoMaSoFilterId').val()
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
                                    _viewDayKeModal.open({ id: data.record.dayKe.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.dayKe.id });                                
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
                                    entityId: data.record.dayKe.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteDayKe(data.record.dayKe);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "dayKe.maSo",
						 name: "maSo"   
					},
					{
						targets: 2,
						 data: "dayKe.ten",
						 name: "ten"   
					},
					{
						targets: 3,
						 data: "phongKhoMaSo" ,
						 name: "phongKhoFk.maSo" 
					}
            ]
        });

        function getDayKes() {
            dataTable.ajax.reload();
        }

        function deleteDayKe(dayKe) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _dayKesService.delete({
                            id: dayKe.id
                        }).done(function () {
                            getDayKes(true);
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

        $('#CreateNewDayKeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _dayKesService
                .getDayKesToExcel({
				filter : $('#DayKesTableFilter').val(),
					maSoFilter: $('#MaSoFilterId').val(),
					tenFilter: $('#TenFilterId').val(),
					phongKhoMaSoFilter: $('#PhongKhoMaSoFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDayKeModalSaved', function () {
            getDayKes();
        });

		$('#GetDayKesButton').click(function (e) {
            e.preventDefault();
            getDayKes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getDayKes();
		  }
		});
    });
})();