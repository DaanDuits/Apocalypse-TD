using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NewGameMenu : MonoBehaviour
{
    public MainMenu mainMenu;

    public TMP_Dropdown diff;
    public TMP_InputField seed;

    string randomText = "handle carriage mighty vulgar dog search crowd deep sigh count mellow please tour shop bruise spooky mom overjoyed observant pricey cabbage white certain anger absorbed unadvised zipper silver mean hideous spoon alarm zip smooth sense club agree scissors two loving wise punish long-term snail blushing known threatening rare stay ducks north green nosy wide deceive shiver sheep alluring various ten rotten cover tug ill wine remarkable activity frequent damaging card queen flowers absorbing milky squeamish help hysterical yard look homely cemetery cloudy base color mother idea governor obtain nauseating cherry airplane wandering file plot rejoice kiss month succeed fluffy soak act foamy preserve clumsy passenger sweltering rate smile robust church planes gaping pig unequaled hop daughter cobweb wacky fall blood appliance befitting eager distinct copy puzzled marble cautious unite jeans imaginary record word flashy frantic inject replace run lackadaisical measly argue quilt frightening trucks scarf practice infamous press steel rural cruel plain pop fold drab page billowy extra-large vagabond brake rhythm gather reward kittens productive paddle industrious parcel spark earthquake shrug ask rest afterthought laughable soap dapper cake salt lamp selective tease buzz glow rob lewd smart bent share remove concerned grey foot plug cheese auspicious nervous bike puny gigantic animated breezy fanatical mist far-flung accessible flash soft strap abashed front premium explode dependent stupendous language grab scrape miscreant heap yoke tasteless pinch capable deeply load placid impress angle chase awake handy successful frighten capricious groan tendency windy expensive cagey birthday bat confuse whole second mind uneven military seal apathetic glove mature head verse weigh store boring mint bucket leg trashy shoe eyes sulky broad scientific stream theory chief aquatic development continue thread library view unequal possessive pancake bloody ashamed school combative gratis cry adaptable work fade numberless recondite paste short baseball example fish oven steam wide-eyed demonic stare cool damage float pickle screw steady object scarce squirrel staking gold aunt handsome expect hope room summer depend erratic ruthless play ice hateful fry war closed meek incandescent elastic cracker political strange plan wool frightened land dust vacuous hands houses ban well-groomed second-hand confess empty trees scent doubtful stamp need valuable pies direction precious judge eight curtain contain temporary three lumpy riddle abounding irritating yak hulking weak chubby muscle shoes cure pigs sad elegant quiver prefer fortunate daily satisfy ticket loud malicious battle cellar creator dizzy electric idiotic border title icy verdant statuesque sleepy blade tiger efficacious brave curve squealing found cooperative wriggle river bell punishment wry happen flock burn pollution intend zinc pest grumpy incompetent voracious half dear married wistful camera neck hobbies skin detect poke geese crawl victorious annoy memory determined flaky magic visitor promise nest shy murky languid ceaseless noiseless digestion spare list unlock solid obese wet observation necessary dynamic animal yarn classy abrupt agreeable chicken breath hose pear picture many basin scorch disagreeable low driving jam utopian evasive sky abaft purple high four aberrant thumb debonair ludicrous unbecoming popcorn scrub joke rescue event best veil cynical crabby regret simplistic neighborly abrasive remain communicate religion waves acid bless poor alcoholic ship colossal faint license nondescript icky stomach left lavish cow lowly humorous connect squash morning print tip announce teeny-tiny peaceful arrive possess marvelous tree nonchalant mug average old-fashioned shock cap seed rampant angry aback concentrate writing receipt hum simple dirt growth stir plausible nasty greet type advice spicy middle quirky elite spotty trains knit interrupt tenuous treat imminent salty futuristic obtainable reproduce drawer expand bleach burly acoustic lean produce festive house eatable choke roll birth sordid internal metal aromatic bright borrow allow redundant recognise comparison stimulating sniff voice instrument bottle wander sea instinctive discussion bubble pack bang parallel corn towering stereotyped painful brawny raise rake uncovered nut march serious interesting brick suspect ordinary use pass wink overt amuse spiteful van pen fit enthusiastic witty ruin challenge well-made spotted fool control lunch grotesque sedate husky lie kneel present playground shake rat fork history light unwritten collar believe noise thought label slimy train friend fresh swim post fetch shelter exultant desk calendar abject lucky plucky film crash basket late tired prose extend profit lock consist signal pastoral dress bawdy improve like move servant care channel swanky wakeful striped rightful sugar heartbreaking discovery subsequent birds curl suit blush abiding crow few male letter permit stiff legal thrill whisper rule meat filthy round responsible itchy garrulous cumbersome massive naughty weary dime trip flesh loaf old earsplitting dull worried stroke join women dispensable overwrought dangerous quince behave obeisant pale decide sink lunchroom sign dock insect glib likeable representative goofy polite jump sock resonant cave radiate materialistic flap legs unit horses collect actually receptive automatic wilderness detail astonishing bells hour scene tedious wiggly dogs adventurous dam judicious shame well-to-do literate madly aftermath wrench aspiring chilly disarm nappy greedy gainful fill ski merciful berserk substance call oafish shaggy surprise bake quartz scream gleaming wing heady shallow useless appear kitty alive polish grease thick cushion juvenile flippant snatch squalid surround lazy unknown humor liquid heat clammy kindhearted achiever ear bikes hole tricky regular courageous early hook lighten remind introduce tame shiny multiply nation permissible nutty ready afraid omniscient throat finger bath scandalous clap impulse tremendous basketball things occur erect water lacking last rambunctious meeting thoughtful level barbarous panicky moan troubled scratch meddle blue-eyed rail vegetable jog quiet warn pushy wooden perpetual silly mundane bore whimsical drain melt sack string travel neat size impossible free check gruesome stuff snake identify rabid deadpan trot hall parsimonious notebook plantation panoramic intelligent strengthen powder glass quack structure motionless zoo painstaking gamy desert broken reign dad thaw supply fearful rot enormous defeated bomb caring minister frail hat bounce worry temper hapless ray absent dreary son pedal design ugly coil guarantee hard-to-find cute knock range mitten elderly can potato faded influence damaged trust fly stranger squeal rush parched point instruct wire reminiscent ragged acidic truthful ignorant hammer colour sassy beautiful committee current harbor synonymous pumped debt car blow reaction grade crack miniature innate heavy trail ad hoc limit edge brief soothe bitter next historical week limping stove coal graceful freezing direful realize sable";

    string[] randomSeed;

    private void Awake()
    {
        randomSeed = randomText.Split(' ');
    }

    public void OnCreateClicked()
    {
        if (seed.text != "")
            PlayerPrefs.SetString("Seed", seed.text);
        else
        {
            System.Random prng = new System.Random();
            PlayerPrefs.SetString("Seed", randomSeed[prng.Next(0, randomSeed.Length)]);
        }
        PlayerPrefs.SetInt("DefaultDifficulty", diff.value);
        DataPersistanceManager.instance.NewGame();

        DataPersistanceManager.instance.SaveGame();
        // load the scene - which will in turn save the game because of OnSceneUnloaded() in the DataPersistanceManager
        SceneManager.LoadSceneAsync("WorldMap");
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}
