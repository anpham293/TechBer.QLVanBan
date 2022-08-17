(function () {
    $(function () {

        var _tieuChiDanhGiasAppService = abp.services.app.tieuChiDanhGias;
        var _entityTypeFullName = 'TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.TieuChiDanhGia';

        var _permissions = {
            create: abp.auth.hasPermission('Pages.TieuChiDanhGias.Create'),
            edit: abp.auth.hasPermission('Pages.TieuChiDanhGias.Edit'),
            'delete': abp.auth.hasPermission('Pages.TieuChiDanhGias.Delete'),
            manageCayTieuChi: abp.auth.hasPermission('Pages.CayTieuChi.ManageTree'),
            cayTieuChi: abp.auth.hasPermission('Pages.CayTIeuChi')
        };

        var _createModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CayTieuChi/CreateModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CayTieuChi/_CreateModal.js',
            modalClass: 'CreateCayTieuChiModal'
        });

        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/CayTieuChi/EditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/CayTieuChi/_EditModal.js',
            modalClass: 'EditCayTieuChiModal'
        });
                
        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();

        function entityHistoryIsEnabled() {
            return abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, function (entityType) {
                    return entityType === _entityTypeFullName;
                }).length === 1;
        }

        class Cay {
            XOA_STATE = {
                XOA_THANH_CONG: 1,
                CON_NODE_CON: 2,
                CO_LOI: 3
            }

            DI_CHUYEN_STATE = {
                THANH_CONG: 1,
                THAT_BAI: 2
            }

            constructor(id, rootBtn) {
                this.eleId = id;
                this.$tree = $('#' + id);
                this.loaiPhuLuc = 0;
                this.show = function () {
                    this.$tree.show();
                };
                this.hide = function () {
                    this.$tree.hide();
                };
                this.unitCount = 0;
                this.$rootBtn = $('#' + rootBtn);
            }

            setUnitCount (unitCount) {
                this.unitCount = unitCount;
                if (unitCount) {
                    this.show();
                } else {
                    this.hide();
                }
            }

            selectedOu = {
                id: null,
                displayName: null,
                code: null,

                set (ouInTree) {
                    if (!ouInTree) {
                        this.selectedOu.id = null;
                        this.selectedOu.displayName = null;
                        this.selectedOu.code = null;
                    } else {
                        this.selectedOu.id = ouInTree.id;
                        this.selectedOu.displayName = ouInTree.original.displayName;
                        this.selectedOu.code = ouInTree.original.code;
                    }

                }
            }

            refreshUnitCount() {
                this.setUnitCount(this.$tree.jstree('get_json').length);
            }

            addUnit(parentId) {
                let self = this

                var instance = $.jstree.reference(self.$tree);

                _createModal.open({
                    parentId: parentId,
                    loaiPhuLuc: self.loaiPhuLuc
                }, function (newOu) {
                    instance.create_node(
                        parentId ? instance.get_node(parentId) : '#',
                        {
                            id: newOu.tieuChiDanhGia.id,
                            parent: newOu.tieuChiDanhGia.parentId ? newOu.tieuChiDanhGia.parentId : '#',
                            code: newOu.tieuChiDanhGia.code,
                            displayName: newOu.tieuChiDanhGia.displayName,
                            memberCount: 0,
                            roleCount: 0,
                            text: self.generateTextOnTree(newOu.tieuChiDanhGia),
                            state: {
                                opened: false
                            }
                        });

                    self.refreshUnitCount();
                });
            }

            generateTextOnTree(ou) {
                var itemClass = ' ou-text-has-members';
                return '<span title="' + ou.code + '" class="ou-text text-dark' + itemClass + '" data-ou-id="' + ou.id + '"><b>' + ou.soThuTu + '</b> '+
                    app.htmlUtils.htmlEncodeText(ou.name) +
                    '&nbsp;&nbsp;(<b>Tối đa ' + ou.diemToiDa + '</b>)' +
                    ' <i class="fa fa-caret-down text-muted"></i> ' +
                    ' <span style="font-size: .82em; opacity: .5;">' +
                    '</span></span>';
            }

            incrementMemberCount(ouId, incrementAmount) {
                var treeNode = this.$tree.jstree('get_node', ouId);
                treeNode.original.memberCount = treeNode.original.memberCount + incrementAmount;
                this.$tree.jstree('rename_node', treeNode, this.generateTextOnTree(treeNode.original));
            }

            incrementRoleCount(ouId, incrementAmount) {
                var treeNode = this.$tree.jstree('get_node', ouId);
                treeNode.original.roleCount = treeNode.original.roleCount + incrementAmount;
                this.$tree.jstree('rename_node', treeNode, this.generateTextOnTree(treeNode.original));
            }

            getTreeDataFromServer(callback) {
                let self = this;
                _tieuChiDanhGiasAppService.getTieuChiForTree(this.loaiPhuLuc).done(function (result) {
                    var treeData = _.map(result, function (item) {
                        return {
                            id: item.tieuChiDanhGia.id,
                            parent: item.tieuChiDanhGia.parentId ? item.tieuChiDanhGia.parentId : '#',
                            code: '0001',
                            displayName: item.tieuChiDanhGia.soThuTu + ' ' + item.tieuChiDanhGia.name,
                            memberCount: 0,
                            roleCount: 0,
                            text: self.generateTextOnTree(item.tieuChiDanhGia),
                            sapXep: item.tieuChiDanhGia.sapXep ?? 0,
                            state: {
                                opened: false
                            }
                        };
                    });

                    callback(treeData);
                });
            }

            init() {
                let self = this;
                abp.ui.setBusy('#' + self.eleId)

                this.loaiPhuLuc = $(self.$tree).attr('loai-phu-luc');


                this.getTreeDataFromServer(function (treeData) {

                    self.setUnitCount(treeData.length)
                    self.$tree.on('changed.jstree', function (e, data) {
                    })
                        .on('move_node.jstree', function (e, data) {

                            console.log(data)

                            var parentNodeName = (!data.parent || data.parent === '#')
                                ? app.localize('Root')
                                : self.$tree.jstree('get_node', data.parent).original.displayName;

                            abp.message.confirm(
                                app.localize('XacNhanDiChuyenTieuChi', data.node.original.displayName, parentNodeName),
                                app.localize('AreYouSure'),
                                function (isConfirmed) {
                                    if (isConfirmed) {
                                        _tieuChiDanhGiasAppService.moveTieuChi({
                                            id: data.node.id,
                                            newParentId: data.parent === '#' ? null : data.parent
                                        }).done(function (ketQua) {
                                            if (ketQua == self.DI_CHUYEN_STATE.THANH_CONG) {
                                                abp.notify.success(app.localize('SuccessfullyMoved'));
                                                self.reload();
                                            }
                                            else {
                                                self.$tree.jstree('refresh'); //rollback
                                                setTimeout(function () { abp.message.error("Di chuyển thất bại"); }, 500);
                                            }
                                        }).fail(function (err) {
                                            self.$tree.jstree('refresh'); //rollback
                                            setTimeout(function () { abp.message.error(err.message); }, 500);
                                        });
                                    } else {
                                        self.$tree.jstree('refresh'); //rollback
                                    }
                                }
                            );
                        })
                        .jstree({
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
                                        editUnit: {
                                            label: app.localize('Edit'),
                                            icon: 'la la-pencil',
                                            _disabled: !_permissions.manageCayTieuChi,
                                            action: function (data) {
                                                var instance = $.jstree.reference(data.reference);
                                                _editModal.open({ id: node.id }, function (updatedOu) {
                                                    self.reload()
                                                    //console.log(node)
                                                    //node.original.sapXep = updatedOu.displayName;
                                                    //instance.rename_node(node, self.generateTextOnTree(updatedOu.tieuChiDanhGia));
                                                });
                                            }
                                        },

                                        addSubUnit: {
                                            label: 'Thêm tiêu chí phụ',
                                            icon: 'la la-plus',
                                            _disabled: !_permissions.manageCayTieuChi,
                                            action: function () {
                                                self.addUnit(node.id);
                                            }
                                        },

                                        'delete': {
                                            label: app.localize("Delete"),
                                            icon: 'la la-remove',
                                            _disabled: !_permissions.manageCayTieuChi,
                                            action: function (data) {
                                                var instance = $.jstree.reference(data.reference);

                                                abp.message.confirm(
                                                    app.localize('XoaTieuChi', node.original.displayName),
                                                    app.localize('AreYouSure'),
                                                    function (isConfirmed) {
                                                        if (isConfirmed) {
                                                            _tieuChiDanhGiasAppService.xoaTieuChi(node.id).done(function (result) {
                                                                if (result == self.XOA_STATE.XOA_THANH_CONG) {
                                                                    abp.notify.success(app.localize('SuccessfullyDeleted'));
                                                                    instance.delete_node(node);
                                                                    self.refreshUnitCount();
                                                                } else if (result == self.XOA_STATE.CON_NODE_CON) {
                                                                    abp.message.warn(
                                                                        'Không thể xóa khi vẫn còn tiêu chí con',
                                                                        'Lưu ý'
                                                                    )
                                                                } else {
                                                                    abp.message.warn(
                                                                        'Có lỗi xảy ra',
                                                                        'Lưu ý'
                                                                    )
                                                                }
                                                                
                                                            }).fail(function (err) {
                                                                setTimeout(function () { abp.message.error(err.message); }, 500);
                                                            });
                                                        }
                                                    }
                                                );
                                            }
                                        }
                                    };

                                    if (entityHistoryIsEnabled()) {
                                        items.history = {
                                            label: app.localize('History'),
                                            icon: 'la la-history',
                                            _disabled: !_permissions.cayTieuChi,
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
                            plugins: [
                                'types',
                                'contextmenu',
                                'wholerow',
                                'sort',
                                'dnd'
                            ]
                        });

                    self.$rootBtn.click(function (e) {
                        console.log(self)
                        e.preventDefault();

                        self.addUnit();
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

        var CayPhuLucIA = new Cay('CayTieuChiIA', 'AddRootUnitButtonIA');
        CayPhuLucIA.init()

        var CayPhuLucIIA = new Cay('CayTieuChiIIA', 'AddRootUnitButtonIIA');
        CayPhuLucIIA.init()

    });
})();