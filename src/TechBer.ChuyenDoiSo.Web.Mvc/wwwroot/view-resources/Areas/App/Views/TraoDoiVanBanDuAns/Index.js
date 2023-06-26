(function () {
    $(function () {

        var _$traoDoiVanBanDuAnsTable = $('#TraoDoiVanBanDuAnsTable');
        var _traoDoiVanBanDuAnsService = abp.services.app.traoDoiVanBanDuAns;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.TraoDoiVanBanDuAn';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.TraoDoiVanBanDuAns.Create'),
            edit: abp.auth.hasPermission('Pages.TraoDoiVanBanDuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.TraoDoiVanBanDuAns.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TraoDoiVanBanDuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/TraoDoiVanBanDuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditTraoDoiVanBanDuAnModal'
        });       

		 var _viewTraoDoiVanBanDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/TraoDoiVanBanDuAns/ViewtraoDoiVanBanDuAnModal',
            modalClass: 'ViewTraoDoiVanBanDuAnModal'
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

        var dataTable = _$traoDoiVanBanDuAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _traoDoiVanBanDuAnsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#TraoDoiVanBanDuAnsTableFilter').val(),
					minNgayGuiFilter:  getDateFilter($('#MinNgayGuiFilterId')),
					maxNgayGuiFilter:  getDateFilter($('#MaxNgayGuiFilterId')),
					noiDungFilter: $('#NoiDungFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					vanBanDuAnNameFilter: $('#VanBanDuAnNameFilterId').val()
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
                                    _viewTraoDoiVanBanDuAnModal.open({ id: data.record.traoDoiVanBanDuAn.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.traoDoiVanBanDuAn.id });                                
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
                                    entityId: data.record.traoDoiVanBanDuAn.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteTraoDoiVanBanDuAn(data.record.traoDoiVanBanDuAn);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "traoDoiVanBanDuAn.ngayGui",
						 name: "ngayGui" ,
					render: function (ngayGui) {
						if (ngayGui) {
							return moment(ngayGui).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 2,
						 data: "traoDoiVanBanDuAn.noiDung",
						 name: "noiDung"   
					},
					{
						targets: 3,
						 data: "userName" ,
						 name: "userFk.name" 
					},
					{
						targets: 4,
						 data: "vanBanDuAnName" ,
						 name: "vanBanDuAnFk.name" 
					}
            ]
        });

        function getTraoDoiVanBanDuAns() {
            dataTable.ajax.reload();
        }

        function deleteTraoDoiVanBanDuAn(traoDoiVanBanDuAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _traoDoiVanBanDuAnsService.delete({
                            id: traoDoiVanBanDuAn.id
                        }).done(function () {
                            getTraoDoiVanBanDuAns(true);
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

        $('#CreateNewTraoDoiVanBanDuAnButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _traoDoiVanBanDuAnsService
                .getTraoDoiVanBanDuAnsToExcel({
				filter : $('#TraoDoiVanBanDuAnsTableFilter').val(),
					minNgayGuiFilter:  getDateFilter($('#MinNgayGuiFilterId')),
					maxNgayGuiFilter:  getDateFilter($('#MaxNgayGuiFilterId')),
					noiDungFilter: $('#NoiDungFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					vanBanDuAnNameFilter: $('#VanBanDuAnNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditTraoDoiVanBanDuAnModalSaved', function () {
            getTraoDoiVanBanDuAns();
        });

		$('#GetTraoDoiVanBanDuAnsButton').click(function (e) {
            e.preventDefault();
            getTraoDoiVanBanDuAns();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getTraoDoiVanBanDuAns();
		  }
		});
    });
})();