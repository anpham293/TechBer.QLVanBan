(function ($) {
    app.modals.ViewChuyenHoSoGiayVanBanDuAn = function () {

        var _chuyenHoSoGiaiesService = abp.services.app.chuyenHoSoGiaies;
        var _$chuyenHoSoGiaiesTable = $('#ChuyenHoSoGiaiesTable');

        var _viewChuyenHoSoGiayModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChuyenHoSoGiaies/ViewchuyenHoSoGiayModal',
            modalClass: 'ViewChuyenHoSoGiayModal'
        });
        
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ChuyenHoSoGiaies/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ChuyenHoSoGiaies/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditChuyenHoSoGiayModal'
        });

        $('#CreateNewChuyenHoSoGiayButton').click(function () {
            _createOrEditModal.open({
                vanBanDuAnId : $('#vanBanDuAnId').val()
            });
        });

        var dataTable = _$chuyenHoSoGiaiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chuyenHoSoGiaiesService.chuyenHoSoGiayVanBanDuAnGetAll,
                inputFilter: function () {
                    return {
                        filter: $('#ChuyenHoSoGiaiesTableFilter').val(),
                        vanBanDuAnId: $('#vanBanDuAnId').val()
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
                                    _viewChuyenHoSoGiayModal.open({ id: data.record.chuyenHoSoGiay.id });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function (data) {
                                    return data.record.chuyenHoSoGiay.trangThai == 0;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.chuyenHoSoGiay.id });
                                }
                            }
                            ]
                    }
                },
                {
                    targets: 1,
                    data: "vanBanDuAnName",
                    name: "vanBanDuAnName"
                },
                {
                    targets: 2,
                    data: "tenNguoiChuyen",
                    name: "nguoiChuyenId"
                },
                {
                    targets: 3,
                    data: "chuyenHoSoGiay.thoiGianChuyen",
                    name: "thoiGianChuyen" ,
                    render: function (thoiGianChuyen) {
                        if (thoiGianChuyen) {
                            return moment(thoiGianChuyen).format('L');
                        }
                        return "";
                    }

                },
                {
                    targets: 4,
                    data: "chuyenHoSoGiay.soLuong",
                    name: "soLuong"
                },
                {
                    targets: 5,
                    data: "chuyenHoSoGiay.trangThai",
                    name: "trangThai",
                    className: "text-center",
                    render: function (data) {
                        var trangThai = "";
                        if(data == app.trangThaiChuyenHoSoGiay.chuaGuiHoSo){
                            trangThai = "<label class='badge badge-danger'>Chưa gửi hồ sơ</label>";
                        }
                        if(data == app.trangThaiChuyenHoSoGiay.dangChoTiepNhan){
                            trangThai = "<label class='badge badge-primary'>Chờ tiếp nhận</label>";
                        }
                        if(data == app.trangThaiChuyenHoSoGiay.daNhanHoSo){
                            trangThai = "<label class='badge badge-success'>Đã nhận hồ sơ</label>";
                        }
                        return trangThai;
                    }
                },
                {
                    targets: 6,
                    data: "chuyenHoSoGiay.thoiGianNhan",
                    name: "thoiGianNhan" ,
                    render: function (thoiGianNhan) {
                        if (thoiGianNhan) {
                            return moment(thoiGianNhan).format('L');
                        }
                        return "";
                    }
                },
                {
                    targets: 7,
                    data: "tenNguoiNhan" ,
                    name: "nguoiNhanFk.name"
                },
                {
                    targets: 8,
                    
                    data: "chuyenHoSoGiay",
                    name: "trangThai",
                    className: "text-center",
                    render: function(chuyenHoSoGiay, type, row, meta){
                        console.log(row);
                        console.log(chuyenHoSoGiay);
                        var guiHoSo = "";
                        var nhanHoSo = "";
                        if(chuyenHoSoGiay.trangThai == app.trangThaiChuyenHoSoGiay.chuaGuiHoSo && row.userId == chuyenHoSoGiay.nguoiChuyenId){
                            guiHoSo = "<a class='text-info guiHoSo' chuyenHoSoGiayId='"+ chuyenHoSoGiay.id +"' style='cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;'><i class=\"fas fa-paper-plane\"></i></a>";
                        }
                        if(chuyenHoSoGiay.trangThai == app.trangThaiChuyenHoSoGiay.dangChoTiepNhan  && row.userId == chuyenHoSoGiay.nguoiNhanId)
                        {
                            nhanHoSo = "<a class='text-success nhanHoSo' chuyenHoSoGiayId='"+ chuyenHoSoGiay.id +"' style='cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;'><i class=\"fas fa-hands-helping\"></i></a>";
                        } 
                        return guiHoSo + " " + nhanHoSo;
                    }
                },
            ]
        });
        
        $(document).on('click', '.guiHoSo', function () {
            var self = $(this);
            var id = self.attr('chuyenHoSoGiayId');
            //trang thai old = chua gui, mew = cho tiep nhan
            var trangThai = app.trangThaiChuyenHoSoGiay.dangChoTiepNhan;
            abp.ui.setBusy($("body"));
            
            abp.message.confirm(
                app.localize("GuiHoSo") + "?",
                app.localize("AreYouSure"),
                function (isConfirmed) {
                    if (isConfirmed){
                        _chuyenHoSoGiaiesService.capNhatTrangThaiChuyenHoSoGiay(id, trangThai).done(function () {
                            getChuyenHoSoGiaies();
                            abp.ui.clearBusy($("body"));
                        });
                    }
                    abp.ui.clearBusy($("body"));
                }
            )
        });

        $(document).on('click', '.nhanHoSo', function () {
            var self = $(this);
            var id = self.attr('chuyenHoSoGiayId');
            //trang thai old = chua gui, mew = cho tiep nhan
            var trangThai = app.trangThaiChuyenHoSoGiay.daNhanHoSo;
            abp.ui.setBusy($("body"));

            abp.message.confirm(
                app.localize("NhanHoSo") + "?",
                app.localize("AreYouSure"),
                function (isConfirmed) {
                    if (isConfirmed){
                        _chuyenHoSoGiaiesService.capNhatTrangThaiChuyenHoSoGiay(id, trangThai).done(function () {
                            getChuyenHoSoGiaies();
                            abp.ui.clearBusy($("body"));
                        });
                    }
                    abp.ui.clearBusy($("body"));
                }
            )
        });
        
        function getChuyenHoSoGiaies() {
            dataTable.ajax.reload();
        }
        abp.event.on('app.createOrEditChuyenHoSoGiayModalSaved', function () {
            getChuyenHoSoGiaies();
        });
    };
})(jQuery);