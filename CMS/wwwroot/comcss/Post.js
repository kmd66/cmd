var quill;
var quillType = 'editor';
var InlineBlot = Quill.import('blots/block');


export class TextEditorExtension {
    static Visibil() {
        if (quillType == 'editor')
            TextEditorExtension.Hide();
        else
            TextEditorExtension.Show();

    }
    static async Hide() {
        quillType = 'code';

        $("#editor").hide();
        $("#InsertImge").hide();
        $(".ql-toolbar").hide();

        var text = await TextEditor.getContents();
        //text = text.replaceAll('<', '&lt;').replaceAll('>', '&gt;');
        $('textarea#editorTextarea').val(text);

        $("#editorTextarea").show();
    }
    static async Show() {
        quillType = 'editor';

        var text = $('textarea#editorTextarea').val();
        await TextEditor.setContents(text);

        $("#editor").show();
        $("#InsertImge").show();
        $(".ql-toolbar").show();

        $("#editorTextarea").hide();

    }
}

class ImageBlot extends InlineBlot {
    static create(data) {
        const node = super.create(data);
        node.setAttribute('src', data.url);
        node.setAttribute('class', data.imgClass);
        node.setAttribute('style', data.imgStyle);
        node.setAttribute('alt', data.imgArt);
        node.setAttribute('slide', data.imgSlideClass);
        return node;
    }
    static value(domNode) {
        const { src, custom } = domNode.dataset;
        return { src, custom };
    }
}

export class TextEditor {
    static quill;
    static Extension = TextEditorExtension;

    static init() {
        var toolbarOptions = [
            ['bold', 'italic', 'underline', 'strike'],
            ['blockquote', 'code-block'],

            //[{ 'header': 1 }, { 'header': 2 }],    // custom button values
            [{ 'list': 'ordered' }, { 'list': 'bullet' }],
            [{ 'script': 'sub' }, { 'script': 'super' }],     // superscript/subscript
            [{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
            [{ 'direction': 'rtl' }],              // text direction

            //[{ 'size': ['small', false, 'large', 'huge'] }],
            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

            [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
            //[{ 'font': ['tahoma', 'tahoma'] }],
            [{ 'align': [] }],
            ['link', 'video'],
            ['clean']                                         // remove formatting button
        ];

        ImageBlot.blotName = 'imageBlot';
        ImageBlot.className = 'image-blot';
        ImageBlot.tagName = 'img';
        Quill.register({ 'formats/imageBlot': ImageBlot });

        quill = new Quill('#editor', {
            modules: {
                toolbar: toolbarOptions
            },
            theme: 'snow'
        });
        TextEditor.textChange();
        TextEditor.descriptionChange($("#descriptionTextarea").val());

        quill.on('editor-change', function (eventName, ...args) {
            if (eventName === 'text-change') {
               TextEditor.textChange();
            } else if (eventName === 'selection-change') {
                TextEditor.selectionChange();
            }
        });
        $("#descriptionTextarea").on("change keyup paste", function () {
            TextEditor.descriptionChange($(this).val());
        });
    }

    static descriptionChange(val) {
        setTimeout(() => {
            var countWord = val.trim().split(/\s+/).length;
            $("#descriptionwordCount").text(countWord);
            $("#descriptionCount").text(val.length);
        }, 50);
    }

    static textChange() {
        setTimeout(() => {
            var text = quill.getText();
            var countWord = text.trim().split(/\s+/).length;
            //var length = quill.getLength();
            $("#wordCount").text(countWord);
            $("#textCount").text(text.length);
        }, 50);
    }

    static selectionChange() {
    }

    static dispose() {
        quill = null;
        var eceditor = document.getElementById('editor');
        if (eceditor)
            eceditor.parentNode.removeChild(eceditor);
        var paras = document.getElementsByClassName('ql-toolbar');

        if (paras) {
            while (paras[0]) {
                paras[0].parentNode.removeChild(paras[0]);
            }
        }

    }

    static async getContents() {
        var html = quill.root.innerHTML;
        return html;
    }

    static async setContents(data) {
        quill.root.innerHTML = data;
    }

    static insertEmbed(data) {
        quill.focus();
        var range = quill.getSelection();
        if (!range) {
            range = { index: 0 };
        }
        if (range.length && range.length > 0)
            quill.deleteText(range);

        quill.insertEmbed(range.index, 'imageBlot', data, 'user');
         //quill.insertEmbed(range.index, 'image', data.url);

    }

}

window.TextEditor = TextEditor;
