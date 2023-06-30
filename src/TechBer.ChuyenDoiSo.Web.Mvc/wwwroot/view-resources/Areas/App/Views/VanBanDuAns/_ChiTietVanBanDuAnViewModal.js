(function ($) {
    app.modals.ChiTietVanBanDuAnViewModal = function () {
    //Phần khai báo và biến chung
        var vanBanDuAnId = $('#VanBanDuAnId').val();
        this.init = function(){
            getHienThiTraoDoi();
        };
    //Phần Báo cáo tiến độ    
        var _$baoCaoVanBanDuAnsTable = $('#BaoCaoVanBanDuAnsTable');
        var _baoCaoVanBanDuAnsService = abp.services.app.baoCaoVanBanDuAns;
        var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QLVB.BaoCaoVanBanDuAn';

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
                                    _viewBaoCaoVanBanDuAnModal.open({id: data.record.baoCaoVanBanDuAn.id});
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({id: data.record.baoCaoVanBanDuAn.id});
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
            _createOrEditModal.open({vanBanDuAnId: vanBanDuAnId});
        });
        
        abp.event.on('app.createOrEditBaoCaoVanBanDuAnModalSaved', function () {
            getBaoCaoVanBanDuAns();
        });

        $('#GetBaoCaoVanBanDuAnsButton').click(function (e) {
            e.preventDefault();
            getBaoCaoVanBanDuAns();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getBaoCaoVanBanDuAns();
            }
        });
        
    //Phần trao đổi văn bản dự án
        
        var _traoDoiVanBanDuAnsService = abp.services.app.traoDoiVanBanDuAns;

        //Hiển thị trao đổi
        
        function getHienThiTraoDoi() {
            _traoDoiVanBanDuAnsService.getHienThiTraoDoi(vanBanDuAnId).done(function (data) {
                function scrolldiv(e) {
                    $('#' + e).scrollTop(1000000);
                }

                $(document).ready(function () {
                    scrolldiv("media-list");
                    scrolldiv("todotl");
                });
                var hienThi = '';
                hienThi +=
                    '<div class="form-group"><div class="col-md-12" style="height: 60vh; overflow-y: scroll">' +
                    '<ul class="media-list" id="media-list">';

                $.each(data.listTraoDoiVanBanDuAn, function (i) {
                    hienThi +=
                        '<li class="media">'+
                        '<div class="media-body todo-comment">'+
                        '<p class="todo-comment-head">'+
                        '<span class="todo-comment-username">'+ data.listTraoDoiVanBanDuAn[i].noiDung +'</span>'+
                        '</p>'+
                        '</div>'+
                        '</li>';
                });
                hienThi += '</ul></div></div>';
                $('#hienThiTraoDoi').html(hienThi);
            });
            $('#noiDung').val('');

            
        }
        
        //Gửi trao đổi
        $('#guiTraoDoi').click(function () {
            _traoDoiVanBanDuAnsService.guiTraoDoi(
                {
                    noiDung: $('#noiDung').val(),
                    vanBanDuAnId: vanBanDuAnId
                }
            ).done(function () {
                getHienThiTraoDoi();
            })
        });
        
    //Phần Thông tin hồ sơ
        //Hien thi File Van ban
            var _viewVanBanDuAnModal = new app.ModalManager({
                viewUrl: abp.appPath + 'App/VanBanDuAns/ViewvanBanDuAnModal',
                modalClass: 'ViewVanBanDuAnModal',
                modalSize: "modal-xl"
            });
            var fileVanBanDuAn = $('#FileVanBanDuAn').val();
            if(fileVanBanDuAn == null || fileVanBanDuAn == ''){
                $('#hienThiFileVanBanDuAn').html('');
            }
            else 
            {
                var fileVBDA = "<a class='text-warning text-bold link-view' data-target='" + vanBanDuAnId + "' style='cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;'><i class=\"fas fa-file-pdf\"></i></a>";
                $('#hienThiFileVanBanDuAn').html(fileVBDA);
            }
            $(document).off("click", ".link-view").on("click", ".link-view", function () {
                var self = $(this);
                _viewVanBanDuAnModal.open({id: self.attr("data-target")});
            });
        
        //Hien thi du an
            var _viewDuAnModal = new app.ModalManager({
                viewUrl: abp.appPath + 'App/DuAns/ViewduAnModal',
                modalClass: 'ViewDuAnModal'
            });
            $(document).off("click", ".view-duAn").on("click", ".view-duAn", function () {
                var self = $(this);
                _viewDuAnModal.open({id: self.attr("data-target")});
            });
            
        //Hiển thị File quyết định         
            var fileQuyetDinh = $('#FileQuyetDinh').val();
            var quyetDinhId = $('#QuyetDinhId').val();
            if(fileQuyetDinh == null || fileQuyetDinh == ''){
                $('#hienThiFileQuyetDinh').html('');
            }
            else 
            {
                var jsonFile = JSON.parse(fileQuyetDinh);
                var contentTypeJsonFile = jsonFile.ContentType;
                if(contentTypeJsonFile == 'application/pdf'){
                    var pdf =  "<a class='text-warning text-bold view-pdf' data-target='" + quyetDinhId + "' style='cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;'><i class=\"fas fa-file-pdf\"></i></a>";
                    $('#hienThiFileQuyetDinh').html(pdf);
                }
                else {
                    var word = "<a class='text-info text-bold view-word' data-target='" + quyetDinhId + "' style='cursor:pointer;font-size:25px; font-weight: bolder;margin: 0 5px;'><i class=\"fas fa-file-word\"></i></a>";
                    $('#hienThiFileQuyetDinh').html(word);
                }
            }
            
            var _viewQuyetDinhFileDoc = new app.ModalManager({
                viewUrl: abp.appPath + 'App/QuyetDinhs/ViewQuyetDinhFileDoc',
                modalSize: "modal-xl"
            });
            var _viewQuyetDinhFilePdf = new app.ModalManager({
                viewUrl: abp.appPath + 'App/QuyetDinhs/ViewQuyetDinhFilePdf',
                modalClass: 'ViewQuyetDinhModal',
                modalSize: "modal-xl"
            });
            $(document).off("click", ".view-pdf").on("click", ".view-pdf", function () {
                var self = $(this);
                _viewQuyetDinhFilePdf.open({id: self.attr("data-target")});
            });
            $(document).off("click", ".view-word").on("click", ".view-word", function () {
                var self = $(this);
                _viewQuyetDinhFileDoc.open({id: self.attr("data-target")});
            });
        //Hiển thị chuyển duyệt hồ sơ
            var trangThaiChuyenDuyetHoSo = $('#trangThaiDuyetHoSo').val();
            var ngayGui = $('#ngayGui').val();
            var ngayDuyet = $('#ngayDuyet').val();
            console.log('321',ngayGui);
            if(trangThaiChuyenDuyetHoSo == app.trangThaiDuyetHoSoConst.chuaGuiDuyet){
                var chuaDuyet = "<label class='badge badge-danger'>Chưa gửi duyệt</label>";
                $('#hienThiDuyetHoSo').html(chuaDuyet);
            }
            if(trangThaiChuyenDuyetHoSo == app.trangThaiDuyetHoSoConst.dangChoDuyet){
                var chuaDuyet = "<a class='viewChuyenDuyetHoSo' vanBanDuAnId = '"+ vanBanDuAnId +"'><label class='badge badge-info'>Đang gửi duyệt <br>T.gian gửi: "+ ngayGui +"</label></a>";
                $('#hienThiDuyetHoSo').html(chuaDuyet);
            }
            if(trangThaiChuyenDuyetHoSo == app.trangThaiDuyetHoSoConst.daDuyet){
                var daDuyet = "<a  class='viewChuyenDuyetHoSo' vanBanDuAnId = '"+ vanBanDuAnId +"'><label class='badge badge-success'>Đã duyệt <br>T.gian duyệt: "+ ngayDuyet +" </label></a>";
                $('#hienThiDuyetHoSo').html(daDuyet);
            }
    };
})(jQuery);