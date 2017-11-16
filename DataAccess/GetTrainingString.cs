using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataAccess
{
                                                           // Retrieves a string from a file
                                                           // used to train a neural net
    public class GetTrainingString
    {

                                                           // Contains a path to the file
                                                           // to be used to train a neural net
        String filePath;

                                                           // An array of sentences built from
                                                           // a paragraph in the file
        String[] splitParagraph;

                                                           // The sentence last read in 
                                                           // the paragraph
        int paragraphIndex;

                                                           // Indicates if the file needs to
                                                           // be loaded again
        bool needToReload;

                                                            // Indicates if a new paragraph needs
                                                            // to be read from the file.
        bool needNewParagraph;

                                                            // A StreamReader to retrieve the 
                                                            // contents of the file
        StreamReader fileReader;
        
        public GetTrainingString(String filePath)
        {
            this.filePath = filePath;
            needToReload = true;
            needNewParagraph = true;
        }

                                                            // Retrieves a new sentence from the
                                                            // file this object reads from
        public String getSentence()
        {
            if (needToReload)
            {
                try
                {
                    fileReader = new StreamReader(filePath);
                    needToReload = false;
                }
                catch
                {
                    throw;
                }
            }
            if (fileReader.EndOfStream)
            {
                fileReader.Close();
                needToReload = true;
                                                            // Reports that the end of the file
                                                            // has been reached
                return "END OF FILE";
            }

            if (needNewParagraph)
            {
                String rawParagraph = fileReader.ReadLine();
                while((rawParagraph.Equals("")) && (false == fileReader.EndOfStream))
                {
                    rawParagraph = fileReader.ReadLine();
                }
                if (rawParagraph.Equals(""))
                {
                    needToReload = true;
                    return "END OF FILE";
                }
                paragraphIndex = -1;
                splitParagraph = rawParagraph.Split('.');
                needNewParagraph = false;
            }
            paragraphIndex++;
            String toReturn;
            if (paragraphIndex >= splitParagraph.Count())
            {
                                                            // Reports that the end of the
                                                            // paragraph has been reached
                toReturn = "END OF PARAGRAPH";
                needNewParagraph = true;
            }
            else
            {
                toReturn = splitParagraph[paragraphIndex];
            }
            return toReturn;
        }
    }
}
