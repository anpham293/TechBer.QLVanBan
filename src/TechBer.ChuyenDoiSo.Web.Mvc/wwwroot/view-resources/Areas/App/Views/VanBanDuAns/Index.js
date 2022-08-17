(function () {
    $(function () {

        var _$vanBanDuAnsTable = $('#VanBanDuAnsTable');
        var _vanBanDuAnsService = abp.services.app.vanBanDuAns;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.VanBanDuAn';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.VanBanDuAns.Create'),
            edit: abp.auth.hasPermission('Pages.VanBanDuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.VanBanDuAns.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/VanBanDuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/VanBanDuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditVanBanDuAnModal'
        });       

		 var _viewVanBanDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/VanBanDuAns/ViewvanBanDuAnModal',
            modalClass: 'ViewVanBanDuAnModal'
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

        var dataTable = _$vanBanDuAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _vanBanDuAnsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#VanBanDuAnsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					kyHieuVanBanFilter: $('#KyHieuVanBanFilterId').val(),
					minNgayBanHanhFilter:  getDateFilter($('#MinNgayBanHanhFilterId')),
					maxNgayBanHanhFilter:  getDateFilter($('#MaxNgayBanHanhFilterId')),
					fileVanBanFilter: $('#FileVanBanFilterId').val(),
					duAnNameFilter: $('#DuAnNameFilterId').val(),
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
                                    _viewVanBanDuAnModal.open({ id: data.record.vanBanDuAn.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.vanBanDuAn.id });                                
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
                                    entityId: data.record.vanBanDuAn.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteVanBanDuAn(data.record.vanBanDuAn);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "vanBanDuAn.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "vanBanDuAn.kyHieuVanBan",
						 name: "kyHieuVanBan"   
					},
					{
						targets: 3,
						 data: "vanBanDuAn.ngayBanHanh",
						 name: "ngayBanHanh" ,
					render: function (ngayBanHanh) {
						if (ngayBanHanh) {
							return moment(ngayBanHanh).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 4,
						 data: "vanBanDuAn.fileVanBan",
						 name: "fileVanBan"   
					},
					{
						targets: 5,
						 data: "duAnName" ,
						 name: "duAnFk.name" 
					},
					{
						targets: 6,
						 data: "quyTrinhDuAnName" ,
						 name: "quyTrinhDuAnFk.name" 
					}
            ]
        });

        function getVanBanDuAns() {
            dataTable.ajax.reload();
        }

        function deleteVanBanDuAn(vanBanDuAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _vanBanDuAnsService.delete({
                            id: vanBanDuAn.id
                        }).done(function () {
                            getVanBanDuAns(true);
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

        $('#CreateNewVanBanDuAnButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _vanBanDuAnsService
                .getVanBanDuAnsToExcel({
				filter : $('#VanBanDuAnsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					kyHieuVanBanFilter: $('#KyHieuVanBanFilterId').val(),
					minNgayBanHanhFilter:  getDateFilter($('#MinNgayBanHanhFilterId')),
					maxNgayBanHanhFilter:  getDateFilter($('#MaxNgayBanHanhFilterId')),
					fileVanBanFilter: $('#FileVanBanFilterId').val(),
					duAnNameFilter: $('#DuAnNameFilterId').val(),
					quyTrinhDuAnNameFilter: $('#QuyTrinhDuAnNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditVanBanDuAnModalSaved', function () {
            getVanBanDuAns();
        });

		$('#GetVanBanDuAnsButton').click(function (e) {
            e.preventDefault();
            getVanBanDuAns();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getVanBanDuAns();
		  }
		});
    });
})();