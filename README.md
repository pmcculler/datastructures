# datastructures

These are some tools written while working on personal projects that others may find useful.

SuffixArray: this is an implementation of a suffix array, used for searching bodies of text in O(log n) time.

TrieNode: this is a trie, used for searching bodies of text in O(k) time.

BurstTrie: this is a burst trie, used primarily for sorting large numbers of strings. By keeping related suffixes together, cache misses are minimized and the result is that the burst sort can greatly outperform more traditional string sorting techniques on large data sets.

CuckooHash: a dictionary based on the CuckooHash table. Full IDictionary interface support. Guaranteed constant O(1) time lookup; space efficiency at least 75% (as opposed to off-the-shelf dictionary's 50%). Drawback is that insert time can be 1x-4x longer. Pretty good trade in many circumstances.
