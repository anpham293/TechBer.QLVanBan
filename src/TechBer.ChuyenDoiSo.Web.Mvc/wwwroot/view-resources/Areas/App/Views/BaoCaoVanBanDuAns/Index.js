(function () {
    $(function () {

        var _$baoCaoVanBanDuAnsTable = $('#BaoCaoVanBanDuAnsTable');
        var _baoCaoVanBanDuAnsService = abp.services.app.baoCaoVanBanDuAns;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.BaoCaoVanBanDuAn';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.BaoCaoVanBanDuAns.Create'),
            edit: abp.auth.hasPermission('Pages.BaoCaoVanBanDuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.BaoCaoVanBanDuAns.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BaoCaoVanBanDuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BaoCaoVanBanDuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditBaoCaoVanBanDuAnModal'
        });       

		 var _viewBaoCaoVanBanDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BaoCaoVanBanDuAns/ViewbaoCaoVanBanDuAnModal',
            modalClass: 'ViewBaoCaoVanBanDuAnModal'
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

        var dataTable = _$baoCaoVanBanDuAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _baoCaoVanBanDuAnsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#BaoCaoVanBanDuAnsTableFilter').val(),
					noiDungCongViecFilter: $('#NoiDungCongViecFilterId').val(),
					moTaChiTietFilter: $('#MoTaChiTietFilterId').val(),
					fileBaoCaoFilter: $('#FileBaoCaoFilterId').val(),
					vanBanDuAnNameFilter: $('#VanBanDuAnNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val()
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
                                    _viewBaoCaoVanBanDuAnModal.open({ id: data.record.baoCaoVanBanDuAn.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.baoCaoVanBanDuAn.id });                                
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
                                    entityId: data.record.baoCaoVanBanDuAn.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteBaoCaoVanBanDuAn(data.record.baoCaoVanBanDuAn);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "baoCaoVanBanDuAn.noiDungCongViec",
						 name: "noiDungCongViec"   
					},
					{
						targets: 2,
						 data: "baoCaoVanBanDuAn.moTaChiTiet",
						 name: "moTaChiTiet"   
					},
					{
						targets: 3,
						 data: "baoCaoVanBanDuAn.fileBaoCao",
						 name: "fileBaoCao"   
					},
					{
						targets: 4,
						 data: "vanBanDuAnName" ,
						 name: "vanBanDuAnFk.name" 
					},
					{
						targets: 5,
						 data: "userName" ,
						 name: "userFk.name" 
					}
            ]
        });

        function getBaoCaoVanBanDuAns() {
            dataTable.ajax.reload();
        }

        function deleteBaoCaoVanBanDuAn(baoCaoVanBanDuAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _baoCaoVanBanDuAnsService.delete({
                            id: baoCaoVanBanDuAn.id
                        }).done(function () {
                            getBaoCaoVanBanDuAns(true);
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

        $('#CreateNewBaoCaoVanBanDuAnButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _baoCaoVanBanDuAnsService
                .getBaoCaoVanBanDuAnsToExcel({
				filter : $('#BaoCaoVanBanDuAnsTableFilter').val(),
					noiDungCongViecFilter: $('#NoiDungCongViecFilterId').val(),
					moTaChiTietFilter: $('#MoTaChiTietFilterId').val(),
					fileBaoCaoFilter: $('#FileBaoCaoFilterId').val(),
					vanBanDuAnNameFilter: $('#VanBanDuAnNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditBaoCaoVanBanDuAnModalSaved', function () {
            getBaoCaoVanBanDuAns();
        });

		$('#GetBaoCaoVanBanDuAnsButton').click(function (e) {
            e.preventDefault();
            getBaoCaoVanBanDuAns();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getBaoCaoVanBanDuAns();
		  }
		});
    });
})();