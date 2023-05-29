using SimonVossSearch.Core.Model.Parser;

namespace SimonVossSearch.Core;

public class TfidfVectorizer
{
    public List<Field> Fields;
    public HashSet<string> Terms = new HashSet<string>();
    public double[][] tf;
    public double[] idf;

    public TfidfVectorizer()
    {
        Execute();
    }
    public void Execute()
    {
        var parser = new DataFileParser();
        Fields = parser.Execute();

        var ngram = 2;

        
        var docFreq = new List<Dictionary<string, int>>();
        foreach (var field in Fields)
        {
            Dictionary<string, int> docTerms = new Dictionary<string, int>();
            for (int i = 0; i < field.Value.Length - (ngram - 1); i++)
            {
                var term = field.Value.Substring(i, ngram);
                Terms.Add(term);
                if (docTerms.TryGetValue(term, out var val))
                {
                    docTerms[term] += val + 1;
                }
                else
                {
                    docTerms[term] = 1;
                }
            }

            docFreq.Add(docTerms);
        }

        tf = new double[docFreq.Count][];
        for (int i = 0; i < docFreq.Count; i++)
        {
            var docTf = docFreq[i];
            tf[i] = new double[Terms.Count];
            var index = 0;
            foreach (var term in Terms)
            {
                if (docTf.TryGetValue(term, out var val))
                {
                    tf[i][index] = val;
                }

                index++;
            }
        }

        idf = new double[Terms.Count];
        var idfIndex = 0;
        foreach (var t in Terms)
        {
            var docsCount = 0;
            foreach (var docF in docFreq)
            {
                if (docF.ContainsKey(t))
                {
                    docsCount++;
                }
            }

            idf[idfIndex] = Math.Log((Fields.Count + 1) / (double)(docsCount + 1)) + 1;
            idfIndex++;
        }

        for (int i = 0; i < Fields.Count; i++)
        {
            var totalSum = 0d;
            for (int j = 0; j < Terms.Count; j++)
            {
                tf[i][j] *= idf[j];
                totalSum += tf[i][j] * tf[i][j];
            }

            var normal = Math.Sqrt(1 / totalSum);
            for (int j = 0; j < Terms.Count; j++)
            {
                tf[i][j] *= normal;
            }
        }
        
    }
}