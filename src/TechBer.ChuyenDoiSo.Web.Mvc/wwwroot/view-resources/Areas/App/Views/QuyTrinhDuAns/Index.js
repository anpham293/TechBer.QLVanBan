(function () {
    $(function () {

        var _$quyTrinhDuAnsTable = $('#QuyTrinhDuAnsTable');
        var _quyTrinhDuAnsService = abp.services.app.quyTrinhDuAns;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.QuyTrinhDuAn';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.QuyTrinhDuAns.Create'),
            edit: abp.auth.hasPermission('Pages.QuyTrinhDuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.QuyTrinhDuAns.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/QuyTrinhDuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditQuyTrinhDuAnModal'
        });       

		 var _viewQuyTrinhDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/QuyTrinhDuAns/ViewquyTrinhDuAnModal',
            modalClass: 'ViewQuyTrinhDuAnModal'
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

        var dataTable = _$quyTrinhDuAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _quyTrinhDuAnsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#QuyTrinhDuAnsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					descriptionsFilter: $('#DescriptionsFilterId').val(),
					minSTTFilter: $('#MinSTTFilterId').val(),
					maxSTTFilter: $('#MaxSTTFilterId').val(),
					maQuyTrinhFilter: $('#MaQuyTrinhFilterId').val(),
					ghiChuFilter: $('#GhiChuFilterId').val(),
					loaiDuAnNameFilter: $('#LoaiDuAnNameFilterId').val(),
					quyTrinhDuAnNameFilter: $('#QuyTrinhDuAnNameFilterId').val()
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
                                    _viewQuyTrinhDuAnModal.open({ id: data.record.quyTrinhDuAn.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.quyTrinhDuAn.id });                                
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
                                    entityId: data.record.quyTrinhDuAn.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteQuyTrinhDuAn(data.record.quyTrinhDuAn);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "quyTrinhDuAn.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "quyTrinhDuAn.descriptions",
						 name: "descriptions"   
					},
					{
						targets: 3,
						 data: "quyTrinhDuAn.stt",
						 name: "stt"   
					},
					{
						targets: 4,
						 data: "quyTrinhDuAn.maQuyTrinh",
						 name: "maQuyTrinh"   
					},
					{
						targets: 5,
						 data: "quyTrinhDuAn.ghiChu",
						 name: "ghiChu"   
					},
					{
						targets: 6,
						 data: "loaiDuAnName" ,
						 name: "loaiDuAnFk.name" 
					},
					{
						targets: 7,
						 data: "quyTrinhDuAnName" ,
						 name: "parentFk.name" 
					}
            ]
        });

        function getQuyTrinhDuAns() {
            dataTable.ajax.reload();
        }

        function deleteQuyTrinhDuAn(quyTrinhDuAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _quyTrinhDuAnsService.delete({
                            id: quyTrinhDuAn.id
                        }).done(function () {
                            getQuyTrinhDuAns(true);
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

        $('#CreateNewQuyTrinhDuAnButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _quyTrinhDuAnsService
                .getQuyTrinhDuAnsToExcel({
				filter : $('#QuyTrinhDuAnsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					descriptionsFilter: $('#DescriptionsFilterId').val(),
					minSTTFilter: $('#MinSTTFilterId').val(),
					maxSTTFilter: $('#MaxSTTFilterId').val(),
					maQuyTrinhFilter: $('#MaQuyTrinhFilterId').val(),
					ghiChuFilter: $('#GhiChuFilterId').val(),
					loaiDuAnNameFilter: $('#LoaiDuAnNameFilterId').val(),
					quyTrinhDuAnNameFilter: $('#QuyTrinhDuAnNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditQuyTrinhDuAnModalSaved', function () {
            getQuyTrinhDuAns();
        });

		$('#GetQuyTrinhDuAnsButton').click(function (e) {
            e.preventDefault();
            getQuyTrinhDuAns();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getQuyTrinhDuAns();
		  }
		});
    });
})();