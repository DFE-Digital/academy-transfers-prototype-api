using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentGeneration.Elements;
using DocumentGeneration.Interfaces;

namespace DocumentGeneration.Builders
{
    public class DocumentBodyBuilder : IDocumentBodyBuilder
    {
        private readonly WordprocessingDocument _document;
        private readonly Body _body;
        private OpenXmlElement _previousElement;

        public DocumentBodyBuilder(WordprocessingDocument document, OpenXmlElement previousElement)
        {
            _document = document;
            _body = document.MainDocumentPart.Document.Body;
            _previousElement = previousElement;
        }

        public void AddParagraph(Action<IParagraphBuilder> action)
        {
            var builder = new ParagraphBuilder();
            action(builder);
            var newElement = builder.Build();
            _previousElement.InsertAfterSelf(newElement);
            _previousElement = newElement;
        }

        public void AddParagraph(string text)
        {
            var builder = new ParagraphBuilder();
            builder.AddText(text);
            var newElement = builder.Build();
            _previousElement.InsertAfterSelf(newElement);
            _previousElement = newElement;
        }

        public void AddTable(Action<ITableBuilder> action)
        {
            var builder = new TableBuilder();
            action(builder);
            var newElement = builder.Build();
            _previousElement.InsertAfterSelf(newElement);
            _previousElement = newElement;
        }

        public void AddTable(IEnumerable<TextElement[]> rows)
        {
            var builder = new TableBuilder();
            foreach (var row in rows)
            {
                builder.AddRow(rBuilder => { rBuilder.AddCells(row); });
            }

            var newElement = builder.Build();
            _previousElement.InsertAfterSelf(newElement);
            _previousElement = newElement;
        }

        public void AddNumberedList(Action<IListBuilder> action)
        {
            var builder = new NumberedListBuilder(_document.MainDocumentPart.NumberingDefinitionsPart);
            action(builder);
            var newElements = builder.Build();
            foreach (var element in newElements)
            {
                _previousElement.InsertAfterSelf(element);
                _previousElement = element;
            }
        }

        public void AddBulletedList(Action<IListBuilder> action)
        {
            var builder = new BulletedListBuilder(_document.MainDocumentPart.NumberingDefinitionsPart);
            action(builder);
            var newElements = builder.Build();
            foreach (var element in newElements)
            {
                _previousElement.InsertAfterSelf(element);
                _previousElement = element;
            }
        }

        public void AddHeading(Action<IHeadingBuilder> action)
        {
            var builder = new HeadingBuilder();
            action(builder);
            var newElements = builder.Build();
            foreach (var element in newElements)
            {
                _previousElement.InsertAfterSelf(element);
                _previousElement = element;
            }
        }
    }
}