(function () {
    $(function () {

        var _doiTuongChuyenDoiSosService = abp.services.app.doiTuongChuyenDoiSos;
        var _idDoiTuong = parseInt($('#DongBoTieuChi').data('target'));
        var _$chiTietDanhGiaForm = $('form[name=ChiTietDanhGiaForm]');
        var _$soLieuThongKeTable = $('#upload-table-container table > tbody');
        var formIndexing = 0

        // TODO thiet ke lai phan quyen
        var _permissions = {
            edit: abp.auth.hasPermission('Pages.DoiTuongChuyenDoiSos.Edit'),
            chamDiem: abp.auth.hasPermission('Pages.DoiTuongChuyenDoiSos.ChamDiem')
        };

        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabled() {
            return abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, function (entityType) {
                    return entityType === _entityTypeFullName;
                }).length === 1;
        }

        $('#DongBoTieuChi').click(function (e) {
            e.preventDefault();
            _doiTuongChuyenDoiSosService.capNhatChiTietTieuChiChoDoiTuong(_idDoiTuong)
                .done(function () {
                    location.reload();
                });
        })

        $('#TongHopDiem').click(function (e) {
            e.preventDefault();
            _doiTuongChuyenDoiSosService.tongHopDiemDoiTuong(_idDoiTuong).done(function (result) {
                location.reload()
            })
        })

        class CayChiTiet {
            XOA_STATE = {
                XOA_THANH_CONG: 1,
                CON_NODE_CON: 2,
                CO_LOI: 3
            }

            DI_CHUYEN_STATE = {
                THANH_CONG: 1,
                THAT_BAI: 2
            }

            CHAM_DIEM_FLAG = {
                TU_DANH_GIA: 0,
                HOI_DONG_THAM_DINH: 1
            }

            constructor(id) {
                this.eleId = id;
                this.$tree = $('#' + id);
                this.show = function () {
                    this.$tree.show();
                };
                this.hide = function () {
                    this.$tree.hide();
                };
                this.unitCount = 0;
                this.doiTuong = parseInt(this.$tree.attr('data-target'))
                this.selectedOu = {
                    id: null,
                    displayName: null,
                    code: null
                };
                this.rightPanel = $('#cap-nhat-panel');
                this.chamDiemFlag = parseInt($('#cham-diem-flag').val());
            }

            setUnitCount(unitCount) {
                this.unitCount = unitCount;
                if (unitCount) {
                    this.show();
                } else {
                    this.hide();
                }
            }

            checkSelectedNode() {
                let self = this;
                self.$tree.jstree(true).check_node(self.selectedOu.id)
            }

            getSelectedId() {
                return this.selectedOu.id;
            }

            setSelected(ouInTree) {
                let self = this;
                if (!ouInTree) {
                    self.selectedOu.id = null;
                    self.selectedOu.displayName = null;
                    self.selectedOu.code = null;
                } else {
                    self.selectedOu.id = ouInTree.id;
                    self.selectedOu.displayName = ouInTree.original.displayName;
                    self.selectedOu.code = ouInTree.original.code;
                }

                self.loadRightPanel();
            }

            refreshUnitCount() {
                this.setUnitCount(this.$tree.jstree('get_json').length);
            }

            generateTextOnTree(ou) {
                var itemClass = ' ou-text-has-members';
                var openBold = ou.parentChiTietId == null ? '<b>' : '';
                var closeBold = ou.parentChiTietId == null ? '</b>' : '';
                return '<span class="ou-text text-dark cham-diem-text-name"' + itemClass + ' data-ou-id="' + ou.id + '">' + '<b>' + ou.soThuTu + '</b> '+
                     openBold + app.htmlUtils.htmlEncodeText(ou.tenTieuChi) + closeBold + ' (' + ou.diemToiDa + ')' +
                    '&nbsp;&nbsp;<b class="badge" style="display: inline-block; background-color: #ffee9e;"> ' + (ou.diemTuDanhGia ?? 0) + '</b>' +
                    '&nbsp;&nbsp;<b class="badge" style="display: inline-block; background-color: #ffb3a1;"> ' + (ou.diemHoiDongThamDinh ?? 0) + '</b>' +
                    '&nbsp;&nbsp;<b class="badge" style="display: inline-block; background-color: #9de4cf;"> ' + (ou.diemDatDuoc ?? 0) + '</b>' +
                    ' <span style="font-size: .82em; opacity: .5;">' +
                    '</span></span>';
            }

            getTreeDataFromServer(callback) {
                let self = this;
                _doiTuongChuyenDoiSosService.getAllChiTietDanhGiaDoiTuong(self.doiTuong).done(function (result) {
                    $('#ten-doi-tuong').html(result.doiTuongName)

                    var tongTDG = 0;
                    var tongHDTD = 0;
                    var tongDD = 0;

                    var treeData = _.map(result.danhSach, function (item) {
                        if (!item.parentChiTietId) {
                            tongTDG += item.diemTuDanhGia;
                            tongHDTD += item.diemHoiDongThamDinh;
                            tongDD += item.diemDatDuoc;
                        }

                        var result = {
                            id: item.id,
                            parent: item.parentChiTietId ? item.parentChiTietId : '#',
                            displayName: item.name,
                            soThuTu: item.STT,
                            memberCount: 0,
                            roleCount: 0,
                            text: self.generateTextOnTree(item),
                            state: {
                                opened: false,
                                checkbox_disabled: true,
                            },
                            sapXep: item.sapXep,
                            diemToiDa: item.diemToiDa
                        }

                        if (item.isLeaf === true) {
                            if (self.chamDiemFlag === self.CHAM_DIEM_FLAG.HOI_DONG_THAM_DINH) {
                                result.state.checked = item.isHoiDongThamDinh
                            } else if (self.chamDiemFlag === self.CHAM_DIEM_FLAG.TU_DANH_GIA){
                                result.state.checked = item.isTuDanhGia
                            }
                        }
                        return result;

                    });

                    $('#tong-diem-tdg').html(tongTDG)
                    $('#tong-diem-hdtd').html(tongHDTD)
                    $('#tong-diem-dd').html(tongDD)

                    callback(treeData);
                });
            }

            loadRightPanel() {
                let self = this;
                abp.ui.setBusy('#cap-nhat-panel')
                _doiTuongChuyenDoiSosService.getChiTietDanhGiaForEdit(self.selectedOu.id).done(function (data) {

                    $('#ChiTietDanhGia_Id').val(data.id)
                    $('#SoThuTu-Id').html('Mục.' + data.soThuTu)
                    $('#TieuChiName-Id').html(data.name)
                    $('#PhuongThucDanhGia-Id').html(data.phuongThucDanhGia)
                    $('#DiemToiDa-Id').html(data.diemToiDa)
                    let diemTuDanhGia = $('#ChiTietDanhGia_DiemTuDanhGia').val(data.diemTuDanhGia)
                    let diemHoiDongThamDinh = $('#ChiTietDanhGia_DiemHoiDongThamDinh').val(data.diemHoiDongThamDinh)
                    let diemDatDuoc = $('#ChiTietDanhGia_DiemDatDuoc').val(data.diemDatDuoc)
                    let description = $('#ChiTietDanhGia_Description').val(data.description)
                    let soLieuKeKhai = $('#ChiTietDanhGia_SoLieuKeKhai')
                    let soLieuKeKhaiPanel = $('#SoLieuKeKhaiDinhKem')
                    let inputGroup = $('#chi-tiet-danh-gia-input-group')
                    let capNhatDanhGiaBtn = $('#cap-nhat-danh-gia')

                    _$soLieuThongKeTable.empty();

                    if (data.isLeaf == false) {
                        diemTuDanhGia.prop('disabled', true);
                        diemHoiDongThamDinh.prop('disabled', true);
                        diemDatDuoc.prop('disabled', true);
                        description.prop('disabled', true);
                        soLieuKeKhai.prop('disabled', true);
                        capNhatDanhGiaBtn.hide();
                        capNhatDanhGiaBtn.prop('disabled', true);
                        inputGroup.hide();
                        soLieuKeKhaiPanel.hide();
                    } else {
                        diemTuDanhGia.prop('disabled', self.chamDiemFlag === self.CHAM_DIEM_FLAG.HOI_DONG_THAM_DINH);
                        diemHoiDongThamDinh.prop('disabled', self.chamDiemFlag === self.CHAM_DIEM_FLAG.TU_DANH_GIA);
                        diemDatDuoc.prop('disabled', self.chamDiemFlag === self.CHAM_DIEM_FLAG.TU_DANH_GIA);
                        description.prop('disabled', self.chamDiemFlag === self.CHAM_DIEM_FLAG.HOI_DONG_THAM_DINH);
                        soLieuKeKhai.prop('disabled', self.chamDiemFlag === self.CHAM_DIEM_FLAG.HOI_DONG_THAM_DINH);
                        capNhatDanhGiaBtn.show();
                        capNhatDanhGiaBtn.prop('disabled', false);
                        soLieuKeKhaiPanel.show();
                        // load uploaded file
                        if (data.soLieuKeKhai !== null && data.soLieuKeKhai !== "") {
                            var solieu = JSON.parse(data.soLieuKeKhai);
                            for (let i = 0; i < solieu.length; i++) {
                                formIndexing += 1;
                                let options = {
                                    index: formIndexing,
                                    id: solieu[i].Guid,
                                    noiDung: solieu[i].NoiDung,
                                    oldFileName: solieu[i].FileName,
                                    oldContentType: solieu[i].ContentType
                                }
                                let newRow = taoRow(options);
                                _$soLieuThongKeTable.append(newRow);
                                danhSo();
                            }
                        }

                        inputGroup.show();
                    }

                    self.rightPanel.show();
                }).always(function () {
                    abp.ui.clearBusy($("#cap-nhat-panel"))
                });
            }

            init() {
                let self = this;
                abp.ui.setBusy('#' + self.eleId)

                this.getTreeDataFromServer(function (treeData) {

                    self.setUnitCount(treeData.length)
                    self.$tree.on('select_node.jstree', function (e, data) {
                        if (data.selected.length != 1) {
                            self.setSelected(null);
                        } else {
                            var selectedNode = data.instance.get_node(data.selected[0]);
                            self.setSelected(selectedNode);
                        }
                    }).jstree({
                        'core': {
                            data: treeData,
                            multiple: false,
                            check_callback: function (operation, node, node_parent, node_position, more) {
                                abp.ui.clearBusy();
                                return true;
                            }
                        },
                        types: {
                            "default": {
                                "icon": "fa fa-folder kt--font-warning"
                            },
                            "file": {
                                "icon": "fa fa-file  kt--font-warning"
                            }
                        },
                        contextmenu: {
                            items: function (node) {
                                var items = {
                                    //editUnit: {
                                    //    label: 'Cập nhật',
                                    //    icon: 'la la-pencil',
                                    //    _disabled: !_permissions.edit,
                                    //    action: function (data) {
                                    //        //var instance = $.jstree.reference(data.reference);
                                    //        console.log(node)

                                    //        _editChiTietDanhGiaModal.open({ id: node.id, soThuTu: node.original.soThuTu, name: node.original.displayName }, function (updatedOu) {
                                    //            //console.log('update',updatedOu)
                                    //            //node.original.displayName = updatedOu.displayName;
                                    //            //instance.rename_node(node, self.generateTextOnTree(updatedOu.tieuChiDanhGia));
                                    //            self.reload()
                                    //        });
                                    //    }
                                    //},

                                    //chamDiem: {
                                    //    label: 'Chấm điểm',
                                    //    icon: 'la la-pencil',
                                    //    _disabled: !_permissions.chamDiem,
                                    //    action: function (data) {
                                    //        alert(JSON.stringify(data))
                                    //    }
                                    //}
                                };

                                if (entityHistoryIsEnabled()) {
                                    items.history = {
                                        label: app.localize('History'),
                                        icon: 'la la-history',
                                        _disabled: !_permissions.chamDiem,
                                        action: function () {
                                            _entityTypeHistoryModal.open({
                                                entityTypeFullName: _entityTypeFullName,
                                                entityId: node.original.id,
                                                entityTypeDescription: node.original.displayName
                                            });
                                        }
                                    };
                                }

                                return items;
                            }
                        },
                        sort: function (node1, node2) {
                            if (parseInt(this.get_node(node2).original.sapXep) < parseInt(this.get_node(node1).original.sapXep)) {
                                return 1;
                            }

                            return -1;
                        },
                        checkbox: {
                            //"keep_selected_style": false,
                            "whole_node": false,
                            "three_state": true,
                            "tie_selection": false
                        },
                        plugins: [
                            'types',
                            'contextmenu',
                            'wholerow',
                            'sort',
                            'checkbox',
                            //'dnd'
                        ]
                    });

                    self.$tree.on('click', '.ou-text .fa-caret-down', function (e) {
                        e.preventDefault();

                        var ouId = $(this).closest('.ou-text').attr('data-ou-id');
                        setTimeout(function () {
                            self.$tree.jstree('show_contextmenu', ouId);
                        }, 100);
                    });
                });
            }

            reload() {
                let self = this;
                self.getTreeDataFromServer(function (treeData) {
                    self.setUnitCount(treeData.length);
                    self.$tree.jstree(true).settings.core.data = treeData;
                    self.$tree.jstree('refresh');
                });
            }
        }

        var Tree = new CayChiTiet('ChiTietTieuChi')
        Tree.init();

        function taiVe(guid, fileName, contentType) {
            var link = abp.appPath + 'App/DoiTuongChuyenDoiSos/Download?guid=' + guid + '&contentType=' + contentType
            var a = document.createElement('A');
            a.href = link;
            a.download = fileName;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }

        // start table upload section
        $('#them-tai-lieu').click(function () {
            formIndexing += 1;
            let options = {
                index: formIndexing,
            }
            let newRow = taoRow(options);
            _$soLieuThongKeTable.append(newRow);
            danhSo();
        })

        function danhSo() {
            var tds = _$soLieuThongKeTable.find("tr td.danh-so");
            tds.each(function(index, element){
                element.innerHTML = index + 1;
            })
        }

        function taoRow(options) {
            options = options || {};
            let index = options.index || 1;
            let isNew = options.isNew || false;
            let oldGuid = options.id || "";
            let noiDung = options.noiDung || "";
            let oldFileName = options.oldFileName || "";
            let oldContentType = options.oldContentType || "";

            function processingRow() {
                //row.classList.add('bg-warning')
                row.style.backgroundColor = "LightPink";
                isNew = true;
            }

            function completeRow() {
                //row.classList.remove('bg-warning')
                row.style.backgroundColor = "";
                isNew = false;
            }

            let xoaBtn = document.createElement("span")
            xoaBtn.className = "badge badge-danger mb-2";
            xoaBtn.innerHTML = "Xóa";
            xoaBtn.setAttribute("style", "cursor: pointer")

            let uploadBtn = document.createElement("span")
            uploadBtn.className = "badge badge-primary";
            uploadBtn.innerHTML = "Tải lên";
            uploadBtn.setAttribute("style", "cursor: pointer")

            // row
            let row = document.createElement("tr");
            row.className = "not-availabled";

            // cot 1
            let cot1 = document.createElement("td");
            cot1.className = "danh-so text-center font-weight-bold align-middle";


            // cot 2
            let cot2 = document.createElement("td");
            cot2.innerHTML = noiDung;
            cot2.addEventListener('click', function (e) {
                let inputField = document.createElement('input');
                inputField.value = e.target.innerHTML;
                inputField.placeholder = "Nhập thông tin..."
                inputField.setAttribute("style", "width: 100%")

                sweetAlert({
                    text: "Thông tin số liệu thống kê",
                    content: inputField,
                    buttons: {
                        cancel: "Hủy",
                        ok: {
                            text: "Chấp nhận",
                            value: "confirmed"
                        }
                    },
                }).then(value => {
                    switch (value) {
                        case "confirmed": {
                            if (e.target.innerHTML !== inputField.value) {
                                e.target.innerHTML = inputField.value;
                                noiDung = inputField.value;
                                processingRow()
                            }
                            break;
                        }
                        default:
                            break;
                    }
                });

                $(inputField).focus();
                $(inputField).keypress(function (e) {
                    if (e.which === 13) {
                        $(".swal-button--ok").trigger('click')
                    }
                })

            });

            // cot3
            // function tao form
            function taoForm() {
                let form = document.createElement("form");
                form.className = "form-validation";
                form.setAttribute("method", "POST");
                form.setAttribute("role", "form");
                form.setAttribute("name", "UploadSoLieuThongKe" + index);
                form.setAttribute("action", "UploadSoLieuThongKe");
                return form;
            }

            let cot3 = document.createElement("td");
            let fileName = "";
            let fileToken = "";
            let contentType = "";
            let formUpload = taoForm();

            let uploadInput = document.createElement("input");
            uploadInput.className = "mb-2";
            uploadInput.setAttribute("type", "file");
            uploadInput.setAttribute("name", "UploadSoLieuThongKe_fileUpload");

            $(uploadInput).change(function (e) {
                if (window.FileReader && window.Blob) {
                    // All the File APIs are supported
                    $(formUpload).submit();
                } else {
                    // File and Blob are not supported
                    abp.notify.warn('Trình duyệt không hỗ trợ upload file!')
                }
            })

            formUpload.append(uploadInput);
            $(formUpload).ajaxForm({
                beforeSubmit: function (formData, jqForm, options) {
                    let $fileInput = $(uploadInput);
                    let files = $fileInput.get()[0].files;

                    if (!files.length) {
                        fileName = "";
                        fileToken = "";
                        contentType = "";
                        formUpload.reset();
                        return false;
                    }

                    let file = files[0];

                    // File size check
                    if (file.size > 5242880) { //5MB
                        abp.message.warn(app.localize('FileUpload_Warn_SizeLimit', app.consts.maxFileBytesUserFriendlyValue));
                        fileName = "";
                        fileToken = "";
                        contentType = "";
                        formUpload.reset();
                        return false;
                    }

                    let mimeType = _.filter(formData, { name: 'UploadSoLieuThongKe_fileUpload' })[0].value.type;

                    formData.push({ name: 'FileType', value: mimeType });
                    formData.push({ name: 'FileName', value: file.name });
                    formData.push({ name: 'FileToken', value: app.guid() });
                },
                success: function (response) {
                    if (response.success) {
                        fileName = response.result.fileName;
                        fileToken = response.result.fileToken;
                        contentType = response.result.fileType;
                        processingRow()
                    } else {
                        fileName = "";
                        fileToken = "";
                        contentType = "";
                        abp.notify.warn("Tệp tải lên dung lượng quá lớn, hoặc kết nối không ổn định");
                    }
                }
            });

            cot3.append(formUpload);
            let downloadFile = document.createElement('span');
            downloadFile.className = "badge badge-success";
            downloadFile.setAttribute("style", "cursor: pointer")
            let icon = document.createElement('i');
            icon.className = "fa fa-download";

            if (oldFileName && oldFileName !== "") {
                downloadFile.append(icon);
                downloadFile.append(" " + oldFileName);
                downloadFile.addEventListener('click', function () {
                    taiVe(oldGuid, oldFileName, oldContentType)
                })
                cot3.append(downloadFile);
            }

            // cot4
            let cot4 = document.createElement("td");
            cot4.className = "text-center";

            uploadBtn.addEventListener('click', function (e) {
                if (isNew) {
                    abp.message.confirm(
                        '',
                        app.localize('AreYouSure'),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                // thực hiện upload số liệu thống kê chi tiết
                                let thongTinUpload = {
                                    id: Tree.getSelectedId(),
                                    noiDung: noiDung,
                                    fileName: fileName,
                                    fileType: contentType,
                                    fileToken: fileToken,
                                    oldFile: oldGuid
                                };

                                _doiTuongChuyenDoiSosService.uploadSoLieuThongKe(
                                    thongTinUpload
                                ).done(function (response) {
                                    oldGuid = response.oldFile;
                                    oldFileName = response.oldFileName;
                                    oldContentType = response.oldContentType
                                    $(downloadFile).empty();
                                    downloadFile.append(icon);
                                    downloadFile.append(oldFileName);
                                    completeRow();
                                    console.log(oldFileName)
                                    //fileName = "";
                                    //fileToken = "";
                                    //contentType = "";
                                })
                            }
                        }
                    );
                }
            })

            xoaBtn.addEventListener('click', function () {
                abp.message.confirm(
                    '',
                    app.localize('AreYouSure'),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            let soLieu = {
                                id: Tree.getSelectedId(),
                                guid: oldGuid
                            };

                            // thực hiện xóa số liệu thống kê chi tiết
                            _doiTuongChuyenDoiSosService.xoaSoLieuThongKe(
                                soLieu
                            ).done(function () {
                                row.remove();
                            });
                        }
                    }
                );
            });

            cot4.append(xoaBtn);
            br = document.createElement("br");
            cot4.append(br);
            cot4.append(uploadBtn);

            row.append(cot1);
            row.append(cot2);
            row.append(cot3);
            row.append(cot4);

            return row;
        }

        $('#cap-nhat-danh-gia').click(function (e) {
            e.preventDefault();
            if (!_$chiTietDanhGiaForm.valid()) {
                abp.notify.info('Dữ liệu nhập chưa đúng');
                return;
            }

            var chiTietDanhGia = _$chiTietDanhGiaForm.serializeFormToObject();

            if (!parseInt(chiTietDanhGia.id)) {
                abp.notify.info('Dữ liệu nhập chưa đúng');
                return;
            }

            //chiTietDanhGia.uploadedFileToken = uploadedFileToken;
            //chiTietDanhGia.soLieuKeKhai = fileName;
            //chiTietDanhGia.contentType = contentType;

            _doiTuongChuyenDoiSosService.editChiTietDanhGia(
                chiTietDanhGia
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                Tree.checkSelectedNode();
                //uploadedFileToken = null;
                //contentType = null;
                $('#custom-select-file').html("Chọn file...")
                $('#' + chiTietDanhGia.id + '_anchor').trigger('click');
            });
        })
        // end table upload section
    });
})();