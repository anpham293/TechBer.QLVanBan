(function () {
    $(function () {

        var _$quyTrinhDuAnAssignedsTable = $('#QuyTrinhDuAnAssignedsTable');
        var _quyTrinhDuAnAssignedsService = abp.services.app.quyTrinhDuAnAssigneds;
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

        var dataTable = _$quyTrinhDuAnAssignedsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _quyTrinhDuAnAssignedsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#QuyTrinhDuAnAssignedsTableFilter').val(),
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
                                    _viewQuyTrinhDuAnAssignedModal.open({ id: data.record.quyTrinhDuAnAssigned.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.quyTrinhDuAnAssigned.id });                                
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
                                    entityId: data.record.quyTrinhDuAnAssigned.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteQuyTrinhDuAnAssigned(data.record.quyTrinhDuAnAssigned);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "quyTrinhDuAnAssigned.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "quyTrinhDuAnAssigned.descriptions",
						 name: "descriptions"   
					},
					{
						targets: 3,
						 data: "quyTrinhDuAnAssigned.stt",
						 name: "stt"   
					},
					{
						targets: 4,
						 data: "quyTrinhDuAnAssigned.soVanBanQuyDinh",
						 name: "soVanBanQuyDinh"   
					},
					{
						targets: 5,
						 data: "quyTrinhDuAnAssigned.maQuyTrinh",
						 name: "maQuyTrinh"   
					},
					{
						targets: 6,
						 data: "loaiDuAnName" ,
						 name: "loaiDuAnFk.name" 
					},
					{
						targets: 7,
						 data: "quyTrinhDuAnName" ,
						 name: "quyTrinhDuAnFk.name" 
					},
					{
						targets: 8,
						 data: "quyTrinhDuAnAssignedName" ,
						 name: "parentFk.name" 
					},
					{
						targets: 9,
						 data: "duAnName" ,
						 name: "duAnFk.name" 
					}
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

        abp.event.on('app.createOrEditQuyTrinhDuAnAssignedModalSaved', function () {
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