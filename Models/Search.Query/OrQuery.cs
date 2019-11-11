﻿using System.Collections.Generic;
using System.Linq;
using Search.Index;
using Search.Text;

namespace Search.Query
{
    public class OrQuery : IQueryComponent
    {
        private List<IQueryComponent> mComponents = new List<IQueryComponent>();

        public IReadOnlyList<IQueryComponent> Components => mComponents;

        public OrQuery(IEnumerable<IQueryComponent> components)
        {
            mComponents.AddRange(components);
        }

        /// <summary>
        /// Get Postings
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="processor">Tokene processor</param>
        /// <returns></returns>
        public IList<Posting> GetPostings(IIndex index, ITokenProcessor processor)
        {
            //list of posting lists from all query components to be OR-merged
            List<IList<Posting>> postingLists = new List<IList<Posting>>();
            //for each components
            foreach (IQueryComponent qc in mComponents)
            {
                //get a posting list and add it to the collection
                postingLists.Add(qc.GetPostings(index, processor));
            }

            return Merge.OrMerge(postingLists);
        }

        /// <summary>
        /// Convert this query component to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(" + ", mComponents.Select(c => c.ToString()));
        }
    }
}
