using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Concrete.EntityFramework.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            builder.Property(a => a.Title).IsRequired();
            builder.Property(a => a.Title).HasMaxLength(100);

            builder.Property(a => a.Content).IsRequired();
            builder.Property(a => a.Content).HasColumnType("NVARCHAR(MAX)");

            builder.Property(a => a.Thumbnail).IsRequired();

            builder.Property(a => a.Date).IsRequired();

            builder.Property(a => a.SeoAuthor).IsRequired();
            builder.Property(a => a.SeoAuthor).HasMaxLength(100);

            builder.Property(a => a.SeoDescription).IsRequired();
            builder.Property(a => a.SeoDescription).HasMaxLength(500);

            builder.Property(a => a.SeoTags).IsRequired();
            builder.Property(a => a.SeoTags).HasMaxLength(500);

            builder.Property(a => a.ViewCount).IsRequired();

            builder.Property(a => a.CommentCount).IsRequired();

            builder.Property(a => a.Thumbnail).IsRequired();
            builder.Property(a => a.Thumbnail).HasMaxLength(250);

            builder.Property(a => a.CreatedDate).IsRequired();

            builder.Property(a => a.ModifiedDate).IsRequired();

            builder.Property(a => a.IsActive).IsRequired();
            
            builder.Property(a => a.IsDeleted).IsRequired();

            builder.Property(a => a.CreatedByName).IsRequired();
            builder.Property(a => a.CreatedByName).HasMaxLength(100);

            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(100);

            builder.Property(a => a.Note).HasMaxLength(500);

            builder.HasOne<Category>(a => a.Category).WithMany(c => c.Articles).HasForeignKey(a => a.CategoryId);
            builder.HasOne<User>(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId);

            builder.ToTable("Articles");

            builder.HasData(
                new Article
                {
                    Id = 1,
                    CategoryId = 1,
                    Title = "C# 9.0 ve .NET 5 Yenilikleri",
                    Content =
                        "Lorem Ipsum, dizgi ve bask?? end??strisinde kullan??lan m??g??r metinlerdir. Lorem Ipsum, ad?? bilinmeyen bir matbaac??n??n bir hurufat numune kitab?? olu??turmak ??zere bir yaz?? galerisini alarak kar????t??rd?????? 1500'lerden beri end??stri standard?? sahte metinler olarak kullan??lm????t??r. Be??y??z y??l boyunca varl??????n?? s??rd??rmekle kalmam????, ayn?? zamanda pek de??i??meden elektronik dizgiye de s????ram????t??r. 1960'larda Lorem Ipsum pasajlar?? da i??eren Letraset yapraklar??n??n yay??nlanmas?? ile ve yak??n zamanda Aldus PageMaker gibi Lorem Ipsum s??r??mleri i??eren masa??st?? yay??nc??l??k yaz??l??mlar?? ile pop??ler olmu??tur.",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "C# 9.0 ve .NET 5 Yenilikleri",
                    SeoTags = "C#, C# 9, .NET5, .NET Framework, .NET Core",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "C# 9.0 ve .NET 5 Yenilikleri",
                    UserId = 1,
                    ViewCount = 100,
                    CommentCount = 0
                },
                new Article
                {
                    Id = 2,
                    CategoryId = 2,
                    Title = "C++ 11 ve 19 Yenilikleri",
                    Content =
                        "Yinelenen bir sayfa i??eri??inin okuyucunun dikkatini da????tt?????? bilinen bir ger??ektir. Lorem Ipsum kullanman??n amac??, s??rekli 'buraya metin gelecek, buraya metin gelecek' yazmaya k??yasla daha dengeli bir harf da????l??m?? sa??layarak okunurlu??u art??rmas??d??r. ??u anda bir??ok masa??st?? yay??nc??l??k paketi ve web sayfa d??zenleyicisi, varsay??lan m??g??r metinler olarak Lorem Ipsum kullanmaktad??r. Ayr??ca arama motorlar??nda 'lorem ipsum' anahtar s??zc??kleri ile arama yap??ld??????nda hen??z tasar??m a??amas??nda olan ??ok say??da site listelenir. Y??llar i??inde, bazen kazara, bazen bilin??li olarak (??rne??in mizah kat??larak), ??e??itli s??r??mleri geli??tirilmi??tir.",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "C++ 11 ve 19 Yenilikleri",
                    SeoTags = "C++ 11 ve 19 Yenilikleri",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "C++ 11 ve 19 Yenilikleri",
                    UserId = 1,
                    ViewCount = 295,
                    CommentCount = 0
                },
                new Article
                {
                    Id = 3,
                    CategoryId = 3,
                    Title = "JavaScript ES2019 ve ES2020 Yenilikleri",
                    Content =
                        "Yayg??n inanc??n tersine, Lorem Ipsum rastgele s??zc??klerden olu??maz. K??kleri M.??. 45 tarihinden bu yana klasik Latin edebiyat??na kadar uzanan 2000 y??ll??k bir ge??mi??i vard??r. Virginia'daki Hampden-Sydney College'dan Latince profes??r?? Richard McClintock, bir Lorem Ipsum pasaj??nda ge??en ve anla????lmas?? en g???? s??zc??klerden biri olan 'consectetur' s??zc??????n??n klasik edebiyattaki ??rneklerini inceledi??inde kesin bir kayna??a ula??m????t??r. Lorm Ipsum, ??i??ero taraf??ndan M.??. 45 tarihinde kaleme al??nan \"de Finibus Bonorum et Malorum\" (??yi ve K??t??n??n U?? S??n??rlar??) eserinin 1.10.32 ve 1.10.33 say??l?? b??l??mlerinden gelmektedir. Bu kitap, ahlak kuram?? ??zerine bir tezdir ve R??nesans d??neminde ??ok pop??ler olmu??tur. Lorem Ipsum pasaj??n??n ilk sat??r?? olan \"Lorem ipsum dolor sit amet\" 1.10.32 say??l?? b??l??mdeki bir sat??rdan gelmektedir. 1500'lerden beri kullan??lmakta olan standard Lorem Ipsum metinleri ilgilenenler i??in yeniden ??retilmi??tir. ??i??ero taraf??ndan yaz??lan 1.10.32 ve 1.10.33 b??l??mleri de 1914 H. Rackham ??evirisinden al??nan ??ngilizce s??r??mleri e??li??inde ??zg??n bi??iminden yeniden ??retilmi??tir.",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "JavaScript ES2019 ve ES2020 Yenilikleri",
                    SeoTags = "JavaScript ES2019 ve ES2020 Yenilikleri",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "JavaScript ES2019 ve ES2020 Yenilikleri",
                    UserId = 1,
                    ViewCount = 12,
                    CommentCount = 0
                }
                ,
                new Article
                {
                    Id = 4,
                    CategoryId = 4,
                    Title = "Typescript 4.1",
                    Content =
                    $"?? um facto estabelecido de que um leitor ?? distra??do pelo conte??do leg??vel de uma p??gina quando analisa a sua mancha gr??fica. Logo, o uso de Lorem Ipsum leva a uma distribui????o mais ou menos normal de letras, ao contr??rio do uso de 'Conte??do aqui,conte??do aqui'', tornando-o texto leg??vel. Muitas ferramentas de publica????o electr??nica e editores de p??ginas web usam actualmente o Lorem Ipsum como o modelo de texto usado por omiss??o, e uma pesquisa por 'lorem ipsum' ir?? encontrar muitos websites ainda na sua inf??ncia. V??rias vers??es t??m evolu??do ao longo dos anos, por vezes por acidente, por vezes propositadamente (como no caso do humor).",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "Typescript 4.1, Typescript, TYPESCRIPT 2021",
                    SeoTags = "Typescript 4.1 G??ncellemeleri",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "Typescript 4.1 Yenilikleri",
                    UserId = 1,
                    ViewCount = 666,
                    CommentCount = 0
                }
                ,
                new Article
                {
                    Id = 5,
                    CategoryId = 5,
                    Title = "Java ve Android'in Gelece??i | 2021",
                    Content =
                        "Yayg??n inanc??n tersine, Lorem Ipsum rastgele s??zc??klerden olu??maz. K??kleri M.??. 45 tarihinden bu yana klasik Latin edebiyat??na kadar uzanan 2000 y??ll??k bir ge??mi??i vard??r. Virginia'daki Hampden-Sydney College'dan Latince profes??r?? Richard McClintock, bir Lorem Ipsum pasaj??nda ge??en ve anla????lmas?? en g???? s??zc??klerden biri olan 'consectetur' s??zc??????n??n klasik edebiyattaki ??rneklerini inceledi??inde kesin bir kayna??a ula??m????t??r. Lorm Ipsum, ??i??ero taraf??ndan M.??. 45 tarihinde kaleme al??nan \"de Finibus Bonorum et Malorum\" (??yi ve K??t??n??n U?? S??n??rlar??) eserinin 1.10.32 ve 1.10.33 say??l?? b??l??mlerinden gelmektedir. Bu kitap, ahlak kuram?? ??zerine bir tezdir ve R??nesans d??neminde ??ok pop??ler olmu??tur. Lorem Ipsum pasaj??n??n ilk sat??r?? olan \"Lorem ipsum dolor sit amet\" 1.10.32 say??l?? b??l??mdeki bir sat??rdan gelmektedir. 1500'lerden beri kullan??lmakta olan standard Lorem Ipsum metinleri ilgilenenler i??in yeniden ??retilmi??tir. ??i??ero taraf??ndan yaz??lan 1.10.32 ve 1.10.33 b??l??mleri de 1914 H. Rackham ??evirisinden al??nan ??ngilizce s??r??mleri e??li??inde ??zg??n bi??iminden yeniden ??retilmi??tir.",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "Java, Android, Mobile, Kotlin, Uygulama Geli??tirme",
                    SeoTags = "Java, Mobil, Kotlin, Android, IOS, SWIFT",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "JAVA",
                    UserId = 1,
                    ViewCount = 3225,
                    CommentCount = 0
                }
                ,
                new Article
                {
                    Id = 6,
                    CategoryId = 6,
                    Title = "Python ile Veri Madencili??i | 2021",
                    Content =
                    $"Le Lorem Ipsum est simplement du faux texte employ?? dans la composition et la mise en page avant impression. Le Lorem Ipsum est le faux texte standard de l'imprimerie depuis les ann??es 1500, quand un imprimeur anonyme assembla ensemble des morceaux de texte pour r??aliser un livre sp??cimen de polices de texte. Il n'a pas fait que survivre cinq si??cles, mais s'est aussi adapt?? ?? la bureautique informatique, sans que son contenu n'en soit modifi??. Il a ??t?? popularis?? dans les ann??es 1960 gr??ce ?? la vente de feuilles Letraset contenant des passages du Lorem Ipsum, et, plus r??cemment, par son inclusion dans des applications de mise en page de texte, comme Aldus PageMaker.",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "Python ile Veri Madencili??i",
                    SeoTags = "Python, Veri Madencili??i Nas??l Yap??l??r?",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "Python",
                    UserId = 1,
                    ViewCount = 9999,
                    CommentCount = 0
                }
                ,
                new Article
                {
                    Id = 7,
                    CategoryId = 7,
                    Title = "Php Laravel Ba??lang???? Rehberi | API",
                    Content =
                        $"Contrairement ?? une opinion r??pandue, le Lorem Ipsum n'est pas simplement du texte al??atoire. Il trouve ses racines dans une oeuvre de la litt??rature latine classique datant de 45 av. J.-C., le rendant vieux de 2000 ans. Un professeur du Hampden-Sydney College, en Virginie, s'est int??ress?? ?? un des mots latins les plus obscurs, consectetur, extrait d'un passage du Lorem Ipsum, et en ??tudiant tous les usages de ce mot dans la litt??rature classique, d??couvrit la source incontestable du Lorem Ipsum. Il provient en fait des sections 1.10.32 et 1.10.33 du 0De Finibus Bonorum et Malorum' (Des Supr??mes Biens et des Supr??mes Maux) de Cic??ron. Cet ouvrage, tr??s populaire pendant la Renaissance, est un trait?? sur la th??orie de l'??thique. Les premi??res lignes du Lorem Ipsum, 'Lorem ipsum dolor sit amet...'', proviennent de la section 1.10.32",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "Php ile API Olu??turma Rehberi",
                    SeoTags = "php, laravel, api, oop",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "PHP",
                    UserId = 1,
                    ViewCount = 4818,
                    CommentCount = 0
                }
                ,
                new Article
                {
                    Id = 8,
                    CategoryId = 8,
                    Title = "Kotlin ile Mobil Programlama",
                    Content =
                        $"Plusieurs variations de Lorem Ipsum peuvent ??tre trouv??es ici ou l??, mais la majeure partie d'entre elles a ??t?? alt??r??e par l'addition d'humour ou de mots al??atoires qui ne ressemblent pas une seconde ?? du texte standard. Si vous voulez utiliser un passage du Lorem Ipsum, vous devez ??tre s??r qu'il n'y a rien d'embarrassant cach?? dans le texte. Tous les g??n??rateurs de Lorem Ipsum sur Internet tendent ?? reproduire le m??me extrait sans fin, ce qui fait de lipsum.com le seul vrai g??n??rateur de Lorem Ipsum. Iil utilise un dictionnaire de plus de 200 mots latins, en combinaison de plusieurs structures de phrases, pour g??n??rer un Lorem Ipsum irr??prochable. Le Lorem Ipsum ainsi obtenu ne contient aucune r??p??tition, ni ne contient des mots farfelus, ou des touches d'humour.",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "Kotlin ile Mobil Programlama Ba??tan Sona Ad??m Ad??m",
                    SeoTags = "kotlin, android, mobil, programlama",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "Kotlin",
                    UserId = 1,
                    ViewCount = 750,
                    CommentCount = 0
                }
                ,
                new Article
                {
                    Id = 9,
                    CategoryId = 9,
                    Title = "Swift ile IOS Programlama",
                    Content =
                        $"Al contrario di quanto si pensi, Lorem Ipsum non ?? semplicemente una sequenza casuale di caratteri. Risale ad un classico della letteratura latina del 45 AC, cosa che lo rende vecchio di 2000 anni. Richard McClintock, professore di latino al Hampden-Sydney College in Virginia, ha ricercato una delle pi?? oscure parole latine, consectetur, da un passaggio del Lorem Ipsum e ha scoperto tra i vari testi in cui ?? citata, la fonte da cui ?? tratto il testo, le sezioni 1.10.32 and 1.10.33 del 'de Finibus Bonorum et Malorum' di Cicerone. Questo testo ?? un trattato su teorie di etica, molto popolare nel Rinascimento. La prima riga del Lorem Ipsum, 'Lorem ipsum dolor sit amet..'', ?? tratta da un passaggio della sezione 1.10.32.",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "Swift ile IOS Mobil Programlama Ba??tan Sona Ad??m Ad??m",
                    SeoTags = "IOS, android, mobil, programlama",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "Swift",
                    UserId = 1,
                    ViewCount = 14900,
                    CommentCount = 0
                }
                ,
                new Article
                {
                    Id = 10,
                    CategoryId = 10,
                    Title = "Ruby on Rails ile AirBnb Klon Kodlayal??m",
                    Content =
                        $"Esistono innumerevoli variazioni dei passaggi del Lorem Ipsum, ma la maggior parte hanno subito delle variazioni del tempo, a causa dell???inserimento di passaggi ironici, o di sequenze casuali di caratteri palesemente poco verosimili. Se si decide di utilizzare un passaggio del Lorem Ipsum, ?? bene essere certi che non contenga nulla di imbarazzante. In genere, i generatori di testo segnaposto disponibili su internet tendono a ripetere paragrafi predefiniti, rendendo questo il primo vero generatore automatico su intenet. Infatti utilizza un dizionario di oltre 200 vocaboli latini, combinati con un insieme di modelli di strutture di periodi, per generare passaggi di testo verosimili. Il testo cos?? generato ?? sempre privo di ripetizioni, parole imbarazzanti o fuori luogo ecc.",
                    Thumbnail = "postImages/defaultThumbnail.jpg",
                    SeoDescription = "Ruby, Ruby on Rails Web Programlama, AirBnb Klon",
                    SeoTags = "Ruby on Rails, Ruby, Web Programlama, AirBnb",
                    SeoAuthor = "Alper Tunga",
                    Date = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "Ruby",
                    UserId = 1,
                    ViewCount = 26777,
                    CommentCount = 0
                }
            );
            
        }
    }
}