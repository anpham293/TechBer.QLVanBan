(function () {
    $(function () {

        var _$loaiDuAnsTable = $('#LoaiDuAnsTable');
        var _loaiDuAnsService = abp.services.app.loaiDuAns;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.LoaiDuAn';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.LoaiDuAns.Create'),
            edit: abp.auth.hasPermission('Pages.LoaiDuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.LoaiDuAns.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LoaiDuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LoaiDuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditLoaiDuAnModal'
        });       

		 var _viewLoaiDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LoaiDuAns/ViewloaiDuAnModal',
            modalClass: 'ViewLoaiDuAnModal'
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

        var dataTable = _$loaiDuAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _loaiDuAnsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#LoaiDuAnsTableFilter').val(),
					nameFilter: $('#NameFilterId').val()
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
                                    _viewLoaiDuAnModal.open({ id: data.record.loaiDuAn.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.loaiDuAn.id });                                
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
                                    entityId: data.record.loaiDuAn.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteLoaiDuAn(data.record.loaiDuAn);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "loaiDuAn.name",
						 name: "name"   
					}
            ]
        });

        function getLoaiDuAns() {
            dataTable.ajax.reload();
        }

        function deleteLoaiDuAn(loaiDuAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _loaiDuAnsService.delete({
                            id: loaiDuAn.id
                        }).done(function () {
                            getLoaiDuAns(true);
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

        $('#CreateNewLoaiDuAnButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _loaiDuAnsService
                .getLoaiDuAnsToExcel({
				filter : $('#LoaiDuAnsTableFilter').val(),
					nameFilter: $('#NameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditLoaiDuAnModalSaved', function () {
            getLoaiDuAns();
        });

		$('#GetLoaiDuAnsButton').click(function (e) {
            e.preventDefault();
            getLoaiDuAns();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getLoaiDuAns();
		  }
		});
    });
})();