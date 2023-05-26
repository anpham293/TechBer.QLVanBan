(function () {
    $(function () {

        var _$duAnsTable = $('#DuAnsTable');
        var _duAnsService = abp.services.app.duAns;

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DuAns.Create'),
            edit: abp.auth.hasPermission('Pages.DuAns.Edit'),
            'delete': abp.auth.hasPermission('Pages.DuAns.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDuAnModal',
            modalSize: "modal-xl"
        });
        
        var _danhSachUserDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/DanhSachUserDuAnModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DuAns/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDuAnModal',
            modalSize: "modal-xl"
        });

        var _viewDuAnModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DuAns/ViewduAnModal',
            modalClass: 'ViewDuAnModal'
        });


        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z");
        }

        var dataTable = _$duAnsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _duAnsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#DuAnsTableFilter').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionsFilter: $('#DescriptionsFilterId').val(),
                        loaiDuAnNameFilter: $('#LoaiDuAnNameFilterId').val()
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
                                text: app.localize('XemChiTiet'),
                                action: function (data) {
                                    _viewDuAnModal.open({id: data.record.duAn.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.duAn.id});
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteDuAn(data.record.duAn);
                                }
                            }]
                    }
                },
                {
                    targets: 1,
                    data: "duAn",
                    name: "name",
                    render: function (duAn) {
                        return '<a style="white-space: normal">'+ duAn.name +'</a>';
                    }
                },
                {
                    targets: 2,
                    data: "duAn.descriptions",
                    name: "descriptions"
                },
                {
                    targets: 3,
                    data: "duAn.tongMucDauTu",
                    name: "tongMucDauTu",
                    className: "text-center",
                    render: function (data) {
                        return '<a>'+ addCommas(data) +' VNĐ</a>' ;
                    }
                },
                {
                    targets: 4,
                    data: "duAn.duToan",
                    name: "duToan",
                    className: "text-center",
                    render: function (data) {
                        return '<a>'+ addCommas(data) +' VNĐ</a>' ;
                    }
                },
                {
                    targets: 5,
                    data: "loaiDuAnName",
                    name: "loaiDuAnFk.name"
                }, 
                {
                    targets: 6,
                    data: "duAn.ngayBatDau",
                    name: "ngayBatDau",
                    className: "text-center",
                    render: function (data) {
                        var ngayBatDau = "";
                        if(data!=null){
                            ngayBatDau = moment(data).format("L");
                        }
                        return ngayBatDau;
                    }
                },
                {
                    targets: 7,
                    data: "duAn.ngayKetThuc",
                    name: "ngayKetThuc",
                    className: "text-center",
                    render: function (data) {
                        var ngayKetThuc = "";
                        if(data!=null){
                            ngayKetThuc = moment(data).format("L");
                        }
                        return ngayKetThuc;
                    }
                    // render: function (data) {
                    //     var html = '<div class="progress progress--sm"><div class="progress-bar kt-bg-danger" role="progressbar" style="width: 100%;" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div> </div>';
                    //     return html;
                    //
                    // }
                    
                },
                {
                    targets: 8,
                    data: "duAn",
                    name: "tienDo",
                    className: "text-center",
                    render: function (data, type, row, meta) {
                        var giaiNgan = row.tongSoTienThanhToan;
                        var tongMucDauTu = row.duAn.tongMucDauTu;
                        var tienDo = (giaiNgan/tongMucDauTu*100).toFixed(2) == 'NaN' ? 0 : (giaiNgan/tongMucDauTu*100).toFixed(2);
                        console.log("175", tienDo);
                        var html = '<div class="progress progress--sm"><div class="progress-bar kt-bg-success" role="progressbar" style="width: '+tienDo +'%;" aria-valuenow="50" aria-valuemin="0" aria-valuemax="100"></div> </div>' +
                                    '<div class="kt-widget24__action"><span class="kt-widget24__number"><span class="counterup">'+tienDo+'</span>%</span> </div>';
                        return html;

                    }

                },
                {
                    targets: 9,
                    data: "duAn.trangThai",
                    className: "text-center",
                    render: function (trangThai, type, row, meta) {
                        let themUser = '<a title="Thêm người vào dự án" style="cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;" class="text-info DanhSachUserDuAn"  duAnId="'+row.duAn.id+'"><i class="fas fa-user-plus"></i></a>';
                        let linkHoSo = "<a href='/App/VanBanDuAns?duanid=" + row.duAn.id + "' style=\"cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;\" title='Mở danh sách hồ sơ'><i class=\"fas fa-file-export\"></i></a>";
                        let stt = '';
                        if(trangThai == app.trangThaiDuAnConst.duAnOpen)
                        {
                            stt = '<a title="Dự án mở, click để đổi trạng thái" style="cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;" class="text-success" data-rowId="'+row.duAn.id+'" data-status="'+row.duAn.trangThai+'"><i class="fas fa-unlock-alt"></i></a>'
                        }
                        if(trangThai == app.trangThaiDuAnConst.duAnClose){
                            stt = '<a title="Dự án đóng, click để đổi trạng thái" style="cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;" class="text-danger" data-rowId="'+row.duAn.id+'" data-status="'+row.duAn.trangThai+'"><i class="fas fa-lock"></i></a>'
                        }
                        return themUser + "&nbsp &nbsp &nbsp" + stt + "&nbsp &nbsp &nbsp" + linkHoSo;
                    }
                }
            ]
        });

        function getDuAns() {
            dataTable.ajax.reload();
        }

        function deleteDuAn(duAn) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _duAnsService.delete({
                            id: duAn.id
                        }).done(function () {
                            getDuAns(true);
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

        $('#CreateNewDuAnButton').click(function () {
            _createOrEditModal.open();
        });
        $(document).on('click','.DanhSachUserDuAn', function () {
            _danhSachUserDuAnModal.open();
        });
        $('#ExportToExcelButton').click(function () {
            _duAnsService
                .getDuAnsToExcel({
                    filter: $('#DuAnsTableFilter').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionsFilter: $('#DescriptionsFilterId').val(),
                    loaiDuAnNameFilter: $('#LoaiDuAnNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDuAnModalSaved', function () {
            getDuAns();
        });

        $('#GetDuAnsButton').click(function (e) {
            e.preventDefault();
            getDuAns();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDuAns();
            }
        });
    });
})();