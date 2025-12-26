using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace CrosswordsPuzzleGenerator.Models;

class CWExportService
{
    public static void CreateWordDocument(string filePath, int gridSize, List<Cell> crossWord)
    {
        if (crossWord == null)
        {
            throw new ArgumentNullException();
        }

        if (crossWord.Count != (gridSize * gridSize))
        {
            throw new ArgumentException("Crossword cell count does not match with the provided grid size.");
        }

        // string _filePath = Path.GetFullPath(@"C:/Users/zanhe/Desktop/testWord.docx");

        if(File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
        {
            // Set the content of the document so that Word can open it.
            MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();

            mainPart.Document = new Document(new Body());

            Table table = new();

            TableProperties props = SetTableProperties();

            table.AppendChild<TableProperties>(props);

            List<Cell>.Enumerator cw = crossWord.GetEnumerator();

            for (int i = 0; i < gridSize;  i++)
            {
                var tr = new TableRow();

                for (int j = 0; j < gridSize; j++)
                {
                    var tc = new TableCell();

                    if (cw.MoveNext() == false)
                    {
                        throw new Exception();
                    }

                    var paragraph = new Paragraph(
                        new ParagraphProperties(
                            new Justification { Val = JustificationValues.Center }
                        ),
                        new Run(new Text(cw.Current.Character.ToString()))
                    );
                    
                    tc.Append(paragraph);

                    tc.Append(new TableCellProperties(
                        new TableCellWidth { Type = TableWidthUnitValues.Dxa , Width = "400" },
                        new TableCellVerticalAlignment {Val = TableVerticalAlignmentValues.Center}));

                    tr.Append(tc);
                }

                tr.Append(new TableRowProperties(new TableRowHeight { Val = 400, HeightType = HeightRuleValues.Exact }));

                table.Append(tr);
            }

            mainPart.Document.Body.Append(table);
        }
    }


    private static TableProperties SetTableProperties()
    {
        TableProperties props = new(
                new TableBorders(
                new TopBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 14
                },
                new BottomBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 14
                },
                new LeftBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 14
                },
                new RightBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 14
                },
                new InsideHorizontalBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 10
                },
                new InsideVerticalBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 10
                }));

        return props;
    }

}

