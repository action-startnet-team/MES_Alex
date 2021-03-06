/*!
 * HTML5 export buttons for Buttons and DataTables.
 * 2015 SpryMedia Ltd - datatables.net/license
 *
 * FileSaver.js (2015-05-07.2) - MIT license
 * Copyright © 2015 Eli Grey - http://eligrey.com
 */
! function(a) {
    "function" == typeof define && define.amd ? define(["jquery", "datatables.net", "datatables.net-buttons"], function(b) {
        return a(b, window, document)
    }) : "object" == typeof exports ? module.exports = function(b, c) {
        return b || (b = window), c && c.fn.dataTable || (c = require("datatables.net")(b, c).$), c.fn.dataTable.Buttons || require("datatables.net-buttons")(b, c), a(c, b, b.document)
    } : a(jQuery, window, document)
}(function(a, b, c, d) {
    "use strict";
    var e = a.fn.dataTable,
        f = function(a) {
            if ("undefined" == typeof navigator || !/MSIE [1-9]\./.test(navigator.userAgent)) {
                var b = a.document,
                    c = function() {
                        return a.URL || a.webkitURL || a
                    },
                    e = b.createElementNS("http://www.w3.org/1999/xhtml", "a"),
                    f = "download" in e,
                    g = function(c) {
                        var d = b.createEvent("MouseEvents");
                        d.initMouseEvent("click", !0, !1, a, 0, 0, 0, 0, 0, !1, !1, !1, !1, 0, null), c.dispatchEvent(d)
                    },
                    h = a.webkitRequestFileSystem,
                    i = a.requestFileSystem || h || a.mozRequestFileSystem,
                    j = function(b) {
                        (a.setImmediate || a.setTimeout)(function() {
                            throw b
                        }, 0)
                    },
                    k = "application/octet-stream",
                    l = 0,
                    m = 500,
                    n = function(b) {
                        var d = function() {
                            "string" == typeof b ? c().revokeObjectURL(b) : b.remove()
                        };
                        a.chrome ? d() : setTimeout(d, m)
                    },
                    o = function(a, b, c) {
                        b = [].concat(b);
                        for (var d = b.length; d--;) {
                            var e = a["on" + b[d]];
                            if ("function" == typeof e) try {
                                e.call(a, c || a)
                            } catch (f) {
                                j(f)
                            }
                        }
                    },
                    p = function(a) {
                        return /^\s*(?:text\/\S*|application\/xml|\S*\/\S*\+xml)\s*;.*charset\s*=\s*utf-8/i.test(a.type) ? new Blob(["\ufeff", a], {
                            type: a.type
                        }) : a
                    },
                    q = function(b, j) {
                        b = p(b);
                        var m, q, r, s = this,
                            t = b.type,
                            u = !1,
                            v = function() {
                                o(s, "writestart progress write writeend".split(" "))
                            },
                            w = function() {
                                if (!u && m || (m = c().createObjectURL(b)), q) q.location.href = m;
                                else {
                                    var e = a.open(m, "_blank");
                                    e === d && "undefined" != typeof safari && (a.location.href = m)
                                }
                                s.readyState = s.DONE, v(), n(m)
                            },
                            x = function(a) {
                                return function() {
                                    return s.readyState !== s.DONE ? a.apply(this, arguments) : void 0
                                }
                            },
                            y = {
                                create: !0,
                                exclusive: !1
                            };
                        return s.readyState = s.INIT, j || (j = "download"), f ? (m = c().createObjectURL(b), e.href = m, e.download = j, g(e), s.readyState = s.DONE, v(), void n(m)) : (a.chrome && t && t !== k && (r = b.slice || b.webkitSlice, b = r.call(b, 0, b.size, k), u = !0), h && "download" !== j && (j += ".download"), (t === k || h) && (q = a), i ? (l += b.size, void i(a.TEMPORARY, l, x(function(a) {
                            a.root.getDirectory("saved", y, x(function(a) {
                                var c = function() {
                                    a.getFile(j, y, x(function(a) {
                                        a.createWriter(x(function(c) {
                                            c.onwriteend = function(b) {
                                                q.location.href = a.toURL(), s.readyState = s.DONE, o(s, "writeend", b), n(a)
                                            }, c.onerror = function() {
                                                var a = c.error;
                                                a.code !== a.ABORT_ERR && w()
                                            }, "writestart progress write abort".split(" ").forEach(function(a) {
                                                c["on" + a] = s["on" + a]
                                            }), c.write(b), s.abort = function() {
                                                c.abort(), s.readyState = s.DONE
                                            }, s.readyState = s.WRITING
                                        }), w)
                                    }), w)
                                };
                                a.getFile(j, {
                                    create: !1
                                }, x(function(a) {
                                    a.remove(), c()
                                }), x(function(a) {
                                    a.code === a.NOT_FOUND_ERR ? c() : w()
                                }))
                            }), w)
                        }), w)) : void w())
                    },
                    r = q.prototype,
                    s = function(a, b) {
                        return new q(a, b)
                    };
                return "undefined" != typeof navigator && navigator.msSaveOrOpenBlob ? function(a, b) {
                    return navigator.msSaveOrOpenBlob(p(a), b)
                } : (r.abort = function() {
                    var a = this;
                    a.readyState = a.DONE, o(a, "abort")
                }, r.readyState = r.INIT = 0, r.WRITING = 1, r.DONE = 2, r.error = r.onwritestart = r.onprogress = r.onwrite = r.onabort = r.onerror = r.onwriteend = null, s)
            }
        }(b),
        g = function(b, c) {
            var e = "*" === b.filename && "*" !== b.title && b.title !== d ? b.title : b.filename;
            return "function" == typeof e && (e = e()), -1 !== e.indexOf("*") && (e = e.replace("*", a("title").text())), e = e.replace(/[^a-zA-Z0-9_\u00A1-\uFFFF\.,\-_ !\(\)]/g, ""), c === d || c === !0 ? e + b.extension : e
        },
        h = function(a) {
            var b = "Sheet1";
            return a.sheetName && (b = a.sheetName.replace(/[\[\]\*\/\\\?\:]/g, "")), b
        },
        i = function(b) {
            var c = b.title;
            return "function" == typeof c && (c = c()), -1 !== c.indexOf("*") ? c.replace("*", a("title").text()) : c
        },
        j = function(a) {
            return a.newline ? a.newline : navigator.userAgent.match(/Windows/) ? "\r\n" : "\n"
        },
        k = function(a, b) {
            for (var c = j(b), e = a.buttons.exportData(b.exportOptions), f = b.fieldBoundary, g = b.fieldSeparator, h = new RegExp(f, "g"), i = b.escapeChar !== d ? b.escapeChar : "\\", k = function(a) {
                    for (var b = "", c = 0, d = a.length; d > c; c++) c > 0 && (b += g), b += f ? f + ("" + a[c]).replace(h, i + f) + f : a[c];
                    return b
                }, l = b.header ? k(e.header) + c : "", m = b.footer && e.footer ? c + k(e.footer) : "", n = [], o = 0, p = e.body.length; p > o; o++) n.push(k(e.body[o]));
            return {
                str: l + n.join(c) + m,
                rows: n.length
            }
        },
        l = function() {
            return -1 !== navigator.userAgent.indexOf("Safari") && -1 === navigator.userAgent.indexOf("Chrome") && -1 === navigator.userAgent.indexOf("Opera")
        },
        m = {
            "_rels/.rels": '<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">	<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/></Relationships>',
            "xl/_rels/workbook.xml.rels": '<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">	<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/></Relationships>',
            "[Content_Types].xml": '<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">	<Default Extension="xml" ContentType="application/xml"/>	<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>	<Default Extension="jpeg" ContentType="image/jpeg"/>	<Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>	<Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/></Types>',
            "xl/workbook.xml": '<?xml version="1.0" encoding="UTF-8" standalone="yes"?><workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">	<fileVersion appName="xl" lastEdited="5" lowestEdited="5" rupBuild="24816"/>	<workbookPr showInkAnnotation="0" autoCompressPictures="0"/>	<bookViews>		<workbookView xWindow="0" yWindow="0" windowWidth="25600" windowHeight="19020" tabRatio="500"/>	</bookViews>	<sheets>		<sheet name="__SHEET_NAME__" sheetId="1" r:id="rId1"/>	</sheets></workbook>',
            "xl/worksheets/sheet1.xml": '<?xml version="1.0" encoding="UTF-8" standalone="yes"?><worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" mc:Ignorable="x14ac" xmlns:x14ac="http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac">	<sheetData>		__DATA__	</sheetData></worksheet>'
        };
    return e.ext.buttons.copyHtml5 = {
        className: "buttons-copy buttons-html5",
        text: function(a) {
            return a.i18n("buttons.copy", "Copy")
        },
        action: function(b, d, e, f) {
            var g = k(d, f),
                h = g.str,
                i = a("<div/>").css({
                    height: 1,
                    width: 1,
                    overflow: "hidden",
                    position: "fixed",
                    top: 0,
                    left: 0
                });
            f.customize && (h = f.customize(h, f));
            var j = a("<textarea readonly/>").val(h).appendTo(i);
            if (c.queryCommandSupported("copy")) {
                i.appendTo(d.table().container()), j[0].focus(), j[0].select();
                try {
                    return c.execCommand("copy"), i.remove(), void d.buttons.info(d.i18n("buttons.copyTitle", "Copy to clipboard"), d.i18n("buttons.copySuccess", {
                        1: "Copied one row to clipboard",
                        _: "Copied %d rows to clipboard"
                    }, g.rows), 2e3)
                } catch (l) {}
            }
            var m = a("<span>" + d.i18n("buttons.copyKeys", "Press <i>ctrl</i> or <i>⌘</i> + <i>C</i> to copy the table data<br>to your system clipboard.<br><br>To cancel, click this message or press escape.") + "</span>").append(i);
            d.buttons.info(d.i18n("buttons.copyTitle", "Copy to clipboard"), m, 0), j[0].focus(), j[0].select();
            var n = a(m).closest(".dt-button-info"),
                o = function() {
                    n.off("click.buttons-copy"), a(c).off(".buttons-copy"), d.buttons.info(!1)
                };
            n.on("click.buttons-copy", o), a(c).on("keydown.buttons-copy", function(a) {
                27 === a.keyCode && o()
            }).on("copy.buttons-copy cut.buttons-copy", function() {
                o()
            })
        },
        exportOptions: {},
        fieldSeparator: "	",
        fieldBoundary: "",
        header: !0,
        footer: !1
    }, e.ext.buttons.csvHtml5 = {
        className: "buttons-csv buttons-html5",
        available: function() {
            return b.FileReader !== d && b.Blob
        },
        text: function(a) {
            return a.i18n("buttons.csv", "CSV")
        },
        action: function(a, b, d, e) {
            var h = (j(e), k(b, e).str),
                i = e.charset;
            e.customize && (h = e.customize(h, e)), i !== !1 ? (i || (i = c.characterSet || c.charset), i && (i = ";charset=" + i)) : i = "", f(new Blob([h], {
                type: "text/csv" + i
            }), g(e))
        },
        filename: "*",
        extension: ".csv",
        exportOptions: {},
        fieldSeparator: ",",
        fieldBoundary: '"',
        escapeChar: '"',
        charset: null,
        header: !0,
        footer: !1
    }, e.ext.buttons.excelHtml5 = {
        className: "buttons-excel buttons-html5",
        available: function() {
            return b.FileReader !== d && b.JSZip !== d && !l()
        },
        text: function(a) {
            return a.i18n("buttons.excel", "Excel")
        },
        action: function(c, e, i, j) {
            var k = "",
                l = e.buttons.exportData(j.exportOptions),
                n = function(b) {
                    for (var c = [], e = 0, f = b.length; f > e; e++) null !== b[e] && b[e] !== d || (b[e] = ""), c.push("number" == typeof b[e] || b[e].match && a.trim(b[e]).match(/^-?\d+(\.\d+)?$/) && "0" !== b[e].charAt(0) ? '<c t="n"><v>' + b[e] + "</v></c>" : '<c t="inlineStr"><is><t>' + (b[e].replace ? b[e].replace(/&(?!amp;)/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/[\x00-\x09\x0B\x0C\x0E-\x1F\x7F-\x9F]/g, "") : b[e]) + "</t></is></c>");
                    return "<row>" + c.join("") + "</row>"
                };
            j.header && (k += n(l.header));
            for (var o = 0, p = l.body.length; p > o; o++) k += n(l.body[o]);
            j.footer && (k += n(l.footer));
            var q = new b.JSZip,
                r = q.folder("_rels"),
                s = q.folder("xl"),
                t = q.folder("xl/_rels"),
                u = q.folder("xl/worksheets");
            q.file("[Content_Types].xml", m["[Content_Types].xml"]), r.file(".rels", m["_rels/.rels"]), s.file("workbook.xml", m["xl/workbook.xml"].replace("__SHEET_NAME__", h(j))), t.file("workbook.xml.rels", m["xl/_rels/workbook.xml.rels"]), u.file("sheet1.xml", m["xl/worksheets/sheet1.xml"].replace("__DATA__", k)), f(q.generate({
                type: "blob",
                mimeType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            }), g(j))
        },
        filename: "*",
        extension: ".xlsx",
        exportOptions: {},
        header: !0,
        footer: !1
    }, e.ext.buttons.pdfHtml5 = {
        className: "buttons-pdf buttons-html5",
        available: function() {
            return b.FileReader !== d && b.pdfMake
        },
        text: function(a) {
            return a.i18n("buttons.pdf", "PDF")
        },
        action: function(c, d, e, h) {
            var k = (j(h), d.buttons.exportData(h.exportOptions)),
                m = [];
            h.header && m.push(a.map(k.header, function(a) {
                return {
                    text: "string" == typeof a ? a : a + "",
                    style: "tableHeader"
                }
            }));
            for (var n = 0, o = k.body.length; o > n; n++) m.push(a.map(k.body[n], function(a) {
                return {
                    text: "string" == typeof a ? a : a + "",
                    style: n % 2 ? "tableBodyEven" : "tableBodyOdd"
                }
            }));
            h.footer && m.push(a.map(k.footer, function(a) {
                return {
                    text: "string" == typeof a ? a : a + "",
                    style: "tableFooter"
                }
            }));
            var p = {
                pageSize: h.pageSize,
                pageOrientation: h.orientation,
                content: [{
                    table: {
                        headerRows: 1,
                        body: m
                    },
                    layout: "noBorders"
                }],
                styles: {
                    tableHeader: {
                        bold: !0,
                        fontSize: 11,
                        color: "white",
                        fillColor: "#2d4154",
                        alignment: "center"
                    },
                    tableBodyEven: {},
                    tableBodyOdd: {
                        fillColor: "#f3f3f3"
                    },
                    tableFooter: {
                      
                      
                        bold: !0,
                        fontSize: 11,
                        color: "white",
                        fillColor: "#2d4154"
                    },
                    title: {
                        alignment: "center",
                        fontSize: 15
                    },
                    message: {}
                },
                defaultStyle: {
                    fontSize: 10,
                    //font: "方正姚体"
                }
            };
            h.message && p.content.unshift({
                text: h.message,
                style: "message",
                margin: [0, 0, 0, 12]
            }), h.title && p.content.unshift({
                text: i(h, !1),
                style: "title",
                margin: [0, 0, 0, 12]
            }), h.customize && h.customize(p, h);
            b.fonts = {
                Roboto: {
                    normal: 'Roboto-Regular.ttf',
                    bold: 'Roboto-Medium.ttf',
                    italics: 'Roboto-Italic.ttf',
                    bolditalics: 'Roboto-Italic.ttf'
                },
                方正姚体: {
                    normal: 'FZYTK.TTF',
                    bold: 'FZYTK.TTF',
                    italics: 'FZYTK.TTF',
                    bolditalics: 'FZYTK.TTF',
                }
                //    微软雅黑: {
                //     normal: '微软雅黑.ttf',
                //     bold: '微软雅黑.ttf',
                //     italics: '微软雅黑.ttf',
                //     bolditalics: '微软雅黑.ttf',
                // }
            };

            var q = b.pdfMake.createPdf(p);
            "open" !== h.download || l() ? q.getBuffer(function(a) {
                var b = new Blob([a], {
                    type: "application/pdf"
                });
                f(b, g(h))
            }) : q.open()
},

        title: "*",
        filename: "*",
        extension: ".pdf",
        exportOptions: {},
        orientation: "portrait",
        pageSize: "A4",
        header: !0,
        footer: !1,
        message: null,
        customize: null,
        download: "download"
    }, e.Buttons
});