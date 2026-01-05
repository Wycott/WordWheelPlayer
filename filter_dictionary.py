#!/usr/bin/env python3

def is_standard_english_word(word):
    """Filter out words that are clearly not standard English dictionary words"""
    
    # Convert to lowercase for checking
    word_lower = word.lower()
    
    # Remove obvious non-dictionary words
    non_standard_patterns = [
        # Abbreviations (3 letters or less, all consonants or mixed)
        lambda w: len(w) <= 3 and w.isupper() and not any(c in 'aeiou' for c in w.lower()),
        
        # Proper nouns (capitalized names, places, etc.)
        lambda w: w in ['aaron', 'aaronic', 'aaronical', 'aaronite', 'aaronitic', 
                       'abraham', 'abrahamic', 'absalom', 'aberdeen', 'acadia', 
                       'acadian', 'abyssinia', 'acadie', 'acapulco', 'abdiel',
                       'abner', 'abbie', 'abbott', 'abel', 'abe'],
        
        # Clear abbreviations
        lambda w: w in ['abc', 'abbr', 'abd', 'abp', 'abr', 'abs', 'abt', 'abv', 'acc'],
        
        # Technical units that are not common words
        lambda w: w in ['abampere', 'abamps', 'abcoulomb', 'abfarad', 'abfarads', 
                       'abhenry', 'abhenries', 'abhenrys', 'abmho', 'abmhos',
                       'abohm', 'abohms', 'abvolt', 'abvolts', 'abwatt', 'abwatts',
                       'absampere', 'absfarad', 'abshenry', 'absmho', 'absohm', 'absvolt'],
        
        # Nonsense or very questionable words
        lambda w: w in ['aaa', 'aarrgh', 'aarrghh', 'ababdeh', 'ababua', 'abacay',
                       'abacaxi', 'abacisci', 'abaciscus', 'aal', 'aalii', 'aaliis',
                       'aals', 'aam', 'aani', 'aaru', 'aas', 'abacli', 'abacot',
                       'abaction', 'abactor', 'abaculi', 'abaculus', 'abada',
                       'abadengo', 'abadia', 'abadite', 'abaff', 'abay', 'abayah',
                       'abaisance', 'abaised', 'abaiser', 'abaisse', 'abaissed',
                       'abaka', 'abakas', 'abalation', 'abama', 'aband', 'abandum',
                       'abanet', 'abanga', 'abanic', 'abantes', 'abapical', 'abarambo',
                       'abaris', 'abas', 'abasgi', 'abasio', 'abask', 'abassi',
                       'abassin', 'abastard', 'abastral', 'abatage', 'abatic',
                       'abatjour', 'abatjours', 'abaton', 'abattage', 'abattu',
                       'abattue', 'abatua', 'abature', 'abaue', 'abave', 'abaze'],
        
        # Foreign words that haven't been adopted into English
        lambda w: w in ['abbaye', 'abbandono', 'abbas', 'abbasi', 'abbasid',
                       'abbassi', 'abbasside', 'abbate', 'abbatie', 'abbe',
                       'abboccato', 'abbogada', 'abbozzo', 'abrazo', 'abrazos',
                       'abrego', 'abreid', 'abreuvoir', 'abri', 'abrico',
                       'abricock', 'abricot', 'abogado', 'abogados'],
        
        # Very technical medical/scientific terms unlikely to be in standard dictionary
        lambda w: w.startswith('ab') and len(w) > 8 and any(x in w for x in ['osis', 'itis', 'emia', 'uria']),
    ]
    
    # Check if word matches any non-standard pattern
    for pattern in non_standard_patterns:
        if pattern(word_lower):
            return False
    
    # Keep words that seem like legitimate English words
    return True

def filter_dictionary():
    input_file = "WordWheelPlayer/words.txt"
    output_file = "WordWheelPlayer/words_filtered.txt"
    
    with open(input_file, 'r', encoding='utf-8') as f:
        words = f.read().splitlines()
    
    # Filter to standard English words
    standard_words = [word for word in words if is_standard_english_word(word)]
    
    # Write filtered words
    with open(output_file, 'w', encoding='utf-8') as f:
        for word in standard_words:
            f.write(word + '\n')
    
    print(f"Original word count: {len(words)}")
    print(f"Filtered word count: {len(standard_words)}")
    print(f"Removed {len(words) - len(standard_words)} non-standard words")

if __name__ == "__main__":
    filter_dictionary()