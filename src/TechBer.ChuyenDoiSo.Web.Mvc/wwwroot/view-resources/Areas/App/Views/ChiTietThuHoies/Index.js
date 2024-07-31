(function () {
    $(function () {

        var _$chiTietThuHoiesTable = $('#ChiTietThuHoiesTable');
        var _chiTietThuHoiesService = abp.services.app.chiTietThuHoies;
		var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.ChiTietThuHoi';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ChiTietThuHoies.Create'),
            edit: abp.auth.hasPermission('Pages.ChiTietThuHoies.Edit'),
            'delete': abp.auth.hasPermission('Pages.ChiTietThuHoies.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChiTietThuHoies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChiTietThuHoies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditChiTietThuHoiModal'
        });       

		 var _viewChiTietThuHoiModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChiTietThuHoies/ViewchiTietThuHoiModal',
            modalClass: 'ViewChiTietThuHoiModal'
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

        var dataTable = _$chiTietThuHoiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chiTietThuHoiesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ChiTietThuHoiesTableFilter').val(),
					minDu1Filter: $('#MinDu1FilterId').val(),
					maxDu1Filter: $('#MaxDu1FilterId').val(),
					minDu2Filter: $('#MinDu2FilterId').val(),
					maxDu2Filter: $('#MaxDu2FilterId').val(),
					minDu3Filter: $('#MinDu3FilterId').val(),
					maxDu3Filter: $('#MaxDu3FilterId').val(),
					minDu4Filter: $('#MinDu4FilterId').val(),
					maxDu4Filter: $('#MaxDu4FilterId').val(),
					minDu5Filter: $('#MinDu5FilterId').val(),
					maxDu5Filter: $('#MaxDu5FilterId').val(),
					minDu6Filter: $('#MinDu6FilterId').val(),
					maxDu6Filter: $('#MaxDu6FilterId').val(),
					minDu7Filter: $('#MinDu7FilterId').val(),
					maxDu7Filter: $('#MaxDu7FilterId').val(),
					minDu8Filter: $('#MinDu8FilterId').val(),
					maxDu8Filter: $('#MaxDu8FilterId').val(),
					minDu9Filter: $('#MinDu9FilterId').val(),
					maxDu9Filter: $('#MaxDu9FilterId').val(),
					minDu10Filter: $('#MinDu10FilterId').val(),
					maxDu10Filter: $('#MaxDu10FilterId').val(),
					minDu11Filter: $('#MinDu11FilterId').val(),
					maxDu11Filter: $('#MaxDu11FilterId').val(),
					minDu12Filter: $('#MinDu12FilterId').val(),
					maxDu12Filter: $('#MaxDu12FilterId').val(),
					minThu1Filter: $('#MinThu1FilterId').val(),
					maxThu1Filter: $('#MaxThu1FilterId').val(),
					minThu2Filter: $('#MinThu2FilterId').val(),
					maxThu2Filter: $('#MaxThu2FilterId').val(),
					minThu3Filter: $('#MinThu3FilterId').val(),
					maxThu3Filter: $('#MaxThu3FilterId').val(),
					minThu4Filter: $('#MinThu4FilterId').val(),
					maxThu4Filter: $('#MaxThu4FilterId').val(),
					minThu5Filter: $('#MinThu5FilterId').val(),
					maxThu5Filter: $('#MaxThu5FilterId').val(),
					minThu6Filter: $('#MinThu6FilterId').val(),
					maxThu6Filter: $('#MaxThu6FilterId').val(),
					minThu7Filter: $('#MinThu7FilterId').val(),
					maxThu7Filter: $('#MaxThu7FilterId').val(),
					minThu8Filter: $('#MinThu8FilterId').val(),
					maxThu8Filter: $('#MaxThu8FilterId').val(),
					minThu9Filter: $('#MinThu9FilterId').val(),
					maxThu9Filter: $('#MaxThu9FilterId').val(),
					minThu10Filter: $('#MinThu10FilterId').val(),
					maxThu10Filter: $('#MaxThu10FilterId').val(),
					minThu11Filter: $('#MinThu11FilterId').val(),
					maxThu11Filter: $('#MaxThu11FilterId').val(),
					minThu12Filter: $('#MinThu12FilterId').val(),
					maxThu12Filter: $('#MaxThu12FilterId').val(),
					ghiChuFilter: $('#GhiChuFilterId').val(),
					tenFilter: $('#TenFilterId').val(),
					danhMucThuHoiTenFilter: $('#DanhMucThuHoiTenFilterId').val()
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
                                    _viewChiTietThuHoiModal.open({ id: data.record.chiTietThuHoi.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.chiTietThuHoi.id });                                
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
                                    entityId: data.record.chiTietThuHoi.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteChiTietThuHoi(data.record.chiTietThuHoi);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "chiTietThuHoi.du1",
						 name: "du1"   
					},
					{
						targets: 2,
						 data: "chiTietThuHoi.du2",
						 name: "du2"   
					},
					{
						targets: 3,
						 data: "chiTietThuHoi.du3",
						 name: "du3"   
					},
					{
						targets: 4,
						 data: "chiTietThuHoi.du4",
						 name: "du4"   
					},
					{
						targets: 5,
						 data: "chiTietThuHoi.du5",
						 name: "du5"   
					},
					{
						targets: 6,
						 data: "chiTietThuHoi.du6",
						 name: "du6"   
					},
					{
						targets: 7,
						 data: "chiTietThuHoi.du7",
						 name: "du7"   
					},
					{
						targets: 8,
						 data: "chiTietThuHoi.du8",
						 name: "du8"   
					},
					{
						targets: 9,
						 data: "chiTietThuHoi.du9",
						 name: "du9"   
					},
					{
						targets: 10,
						 data: "chiTietThuHoi.du10",
						 name: "du10"   
					},
					{
						targets: 11,
						 data: "chiTietThuHoi.du11",
						 name: "du11"   
					},
					{
						targets: 12,
						 data: "chiTietThuHoi.du12",
						 name: "du12"   
					},
					{
						targets: 13,
						 data: "chiTietThuHoi.thu1",
						 name: "thu1"   
					},
					{
						targets: 14,
						 data: "chiTietThuHoi.thu2",
						 name: "thu2"   
					},
					{
						targets: 15,
						 data: "chiTietThuHoi.thu3",
						 name: "thu3"   
					},
					{
						targets: 16,
						 data: "chiTietThuHoi.thu4",
						 name: "thu4"   
					},
					{
						targets: 17,
						 data: "chiTietThuHoi.thu5",
						 name: "thu5"   
					},
					{
						targets: 18,
						 data: "chiTietThuHoi.thu6",
						 name: "thu6"   
					},
					{
						targets: 19,
						 data: "chiTietThuHoi.thu7",
						 name: "thu7"   
					},
					{
						targets: 20,
						 data: "chiTietThuHoi.thu8",
						 name: "thu8"   
					},
					{
						targets: 21,
						 data: "chiTietThuHoi.thu9",
						 name: "thu9"   
					},
					{
						targets: 22,
						 data: "chiTietThuHoi.thu10",
						 name: "thu10"   
					},
					{
						targets: 23,
						 data: "chiTietThuHoi.thu11",
						 name: "thu11"   
					},
					{
						targets: 24,
						 data: "chiTietThuHoi.thu12",
						 name: "thu12"   
					},
					{
						targets: 25,
						 data: "chiTietThuHoi.ghiChu",
						 name: "ghiChu"   
					},
					{
						targets: 26,
						 data: "chiTietThuHoi.ten",
						 name: "ten"   
					},
					{
						targets: 27,
						 data: "danhMucThuHoiTen" ,
						 name: "danhMucThuHoiFk.ten" 
					}
            ]
        });

        function getChiTietThuHoies() {
            dataTable.ajax.reload();
        }

        function deleteChiTietThuHoi(chiTietThuHoi) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _chiTietThuHoiesService.delete({
                            id: chiTietThuHoi.id
                        }).done(function () {
                            getChiTietThuHoies(true);
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

        $('#CreateNewChiTietThuHoiButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _chiTietThuHoiesService
                .getChiTietThuHoiesToExcel({
				filter : $('#ChiTietThuHoiesTableFilter').val(),
					minDu1Filter: $('#MinDu1FilterId').val(),
					maxDu1Filter: $('#MaxDu1FilterId').val(),
					minDu2Filter: $('#MinDu2FilterId').val(),
					maxDu2Filter: $('#MaxDu2FilterId').val(),
					minDu3Filter: $('#MinDu3FilterId').val(),
					maxDu3Filter: $('#MaxDu3FilterId').val(),
					minDu4Filter: $('#MinDu4FilterId').val(),
					maxDu4Filter: $('#MaxDu4FilterId').val(),
					minDu5Filter: $('#MinDu5FilterId').val(),
					maxDu5Filter: $('#MaxDu5FilterId').val(),
					minDu6Filter: $('#MinDu6FilterId').val(),
					maxDu6Filter: $('#MaxDu6FilterId').val(),
					minDu7Filter: $('#MinDu7FilterId').val(),
					maxDu7Filter: $('#MaxDu7FilterId').val(),
					minDu8Filter: $('#MinDu8FilterId').val(),
					maxDu8Filter: $('#MaxDu8FilterId').val(),
					minDu9Filter: $('#MinDu9FilterId').val(),
					maxDu9Filter: $('#MaxDu9FilterId').val(),
					minDu10Filter: $('#MinDu10FilterId').val(),
					maxDu10Filter: $('#MaxDu10FilterId').val(),
					minDu11Filter: $('#MinDu11FilterId').val(),
					maxDu11Filter: $('#MaxDu11FilterId').val(),
					minDu12Filter: $('#MinDu12FilterId').val(),
					maxDu12Filter: $('#MaxDu12FilterId').val(),
					minThu1Filter: $('#MinThu1FilterId').val(),
					maxThu1Filter: $('#MaxThu1FilterId').val(),
					minThu2Filter: $('#MinThu2FilterId').val(),
					maxThu2Filter: $('#MaxThu2FilterId').val(),
					minThu3Filter: $('#MinThu3FilterId').val(),
					maxThu3Filter: $('#MaxThu3FilterId').val(),
					minThu4Filter: $('#MinThu4FilterId').val(),
					maxThu4Filter: $('#MaxThu4FilterId').val(),
					minThu5Filter: $('#MinThu5FilterId').val(),
					maxThu5Filter: $('#MaxThu5FilterId').val(),
					minThu6Filter: $('#MinThu6FilterId').val(),
					maxThu6Filter: $('#MaxThu6FilterId').val(),
					minThu7Filter: $('#MinThu7FilterId').val(),
					maxThu7Filter: $('#MaxThu7FilterId').val(),
					minThu8Filter: $('#MinThu8FilterId').val(),
					maxThu8Filter: $('#MaxThu8FilterId').val(),
					minThu9Filter: $('#MinThu9FilterId').val(),
					maxThu9Filter: $('#MaxThu9FilterId').val(),
					minThu10Filter: $('#MinThu10FilterId').val(),
					maxThu10Filter: $('#MaxThu10FilterId').val(),
					minThu11Filter: $('#MinThu11FilterId').val(),
					maxThu11Filter: $('#MaxThu11FilterId').val(),
					minThu12Filter: $('#MinThu12FilterId').val(),
					maxThu12Filter: $('#MaxThu12FilterId').val(),
					ghiChuFilter: $('#GhiChuFilterId').val(),
					tenFilter: $('#TenFilterId').val(),
					danhMucThuHoiTenFilter: $('#DanhMucThuHoiTenFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditChiTietThuHoiModalSaved', function () {
            getChiTietThuHoies();
        });

		$('#GetChiTietThuHoiesButton').click(function (e) {
            e.preventDefault();
            getChiTietThuHoies();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getChiTietThuHoies();
		  }
		});
    });
})();