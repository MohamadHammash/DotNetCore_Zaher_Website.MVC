using AutoMapper;
using System;
using Zaher.Core.Entities;
using Zaher.Ui.Models.ViewModels.ApplicationUsersViewModels;
using Zaher.Ui.Models.ViewModels.FBooksViewModels;
using Zaher.Ui.Models.ViewModels.PostingCardsViewModels;
using Zaher.Ui.Models.ViewModels.QAsViewModels;
using Zaher.Ui.Models.ViewModels.SectionsViewModels;
using Zaher.Ui.Models.ViewModels.VideosListsViewModels;
using Zaher.Ui.Models.ViewModels.VideosViewModels;

namespace Zaher.Ui.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Videos
            CreateMap<Video, CreateVideoViewModel>().ReverseMap();
            CreateMap<Video, ListVideosViewModel>();
            CreateMap<Video, DetailsVideoViewModel>();

            CreateMap<Video, EditVideoViewModel>();

            CreateMap<EditVideoViewModel, Video>()
                .ForMember(from => from.PublishingDate, act => act.Ignore())
                .ForMember(dest => dest.VideosListId, from => from.MapFrom(vl => (Guid)vl.VideosListId))

                ;

            CreateMap<Video, DeleteVideoViewModel>();

            // Posting Cards

            CreateMap<PostingCard, CreatePostingCardViewModel>().ReverseMap();
            CreateMap<PostingCard, ListPostingCardsViewModel>();
            CreateMap<PostingCard, DetailsPostingCardViewModel>();

            CreateMap<PostingCard, EditPostingCardViewModel>();
            CreateMap<EditPostingCardViewModel, PostingCard>()
                .ForMember(dest => dest.ImagePath, act => act.Ignore())
                .ForMember(dest => dest.PublishingDate, act => act.Ignore())
                ;

            CreateMap<PostingCard, DeletePostingCardViewModel>().ReverseMap();

            // VideosLists
            CreateMap<VideosList, CreateVideosListViewModel>().ReverseMap();
            CreateMap<VideosList, ListVideosListsViewModel>();

            CreateMap<VideosList, EditVideosListViewModel>()
              ;
            CreateMap<EditVideosListViewModel, VideosList>()
                .ForMember(from => from.ImagePath, act => act.Ignore())
                .ForMember(from => from.Videos, act => act.Ignore())
                ;

            CreateMap<VideosList, DetailsVideosListViewModel>().ReverseMap();
            CreateMap<VideosList, DeleteVideosListViewModel>().ReverseMap();

            // QAs

            CreateMap<QA, CreateQAViewModel>().ReverseMap();
            CreateMap<QA, DeleteQAViewModel>().ReverseMap();
            CreateMap<QA, ListQAsViewModel>();
            CreateMap<QA, DetailsQAViewModel>();

            CreateMap<QA, EditQAViewModel>();
            CreateMap<QA, AnswerQAViewModel>();

            CreateMap<EditQAViewModel, QA>()
                .ForMember(from => from.FirstName, act => act.Ignore())
                .ForMember(from => from.LastName, act => act.Ignore())
                .ForMember(from => from.CaseNumber, act => act.Ignore())
                .ForMember(from => from.Email, act => act.Ignore())

                ;

            CreateMap<AnswerQAViewModel, QA>()
                .ForMember(from => from.FirstName, act => act.Ignore())
                .ForMember(from => from.LastName, act => act.Ignore())
                .ForMember(from => from.Subject, act => act.Ignore())
                .ForMember(from => from.Question, act => act.Ignore())
                .ForMember(from => from.CaseNumber, act => act.Ignore())
                .ForMember(from => from.PublishingDate, act => act.Ignore())
                .ForMember(from => from.Email, act => act.Ignore())
                // .ForMember(from => from.SectionId, act => act.Ignore())
                //.ForMember(from => from.Section, act => act.Ignore())
                ;
            CreateMap<QA, MoveQAViewModel>();

            CreateMap<MoveQAViewModel,QA>()
                .ForMember(from => from.FirstName, act => act.Ignore())
                .ForMember(from => from.LastName, act => act.Ignore())
                .ForMember(from => from.Subject, act => act.Ignore())
                .ForMember(from => from.Question, act => act.Ignore())
                .ForMember(from => from.CaseNumber, act => act.Ignore())
                .ForMember(from => from.PublishingDate, act => act.Ignore())
                .ForMember(from => from.Email, act => act.Ignore())
                .ForMember(from => from.Answer, act => act.Ignore())

                ;
            
            // FBooks

            CreateMap<FBook, CreateFBookViewModel>().ReverseMap();
            CreateMap<FBook, DeleteFBookViewModel>().ReverseMap();
            CreateMap<FBook, EditFBookViewModel>();

            CreateMap<EditFBookViewModel, FBook>()
                .ForMember(from => from.Sections, act => act.Ignore())

                ;

            CreateMap<FBook, ListFBooksViewModel>();
            CreateMap<FBook, DetailsFBookViewModel>();

            // Sections

            CreateMap<Section, CreateSectionViewModel>().ReverseMap();
            CreateMap<Section, DeleteSectionViewModel>().ReverseMap();
            CreateMap<Section, EditSectionViewModel>();
            CreateMap<EditSectionViewModel, Section>()
                .ForMember(from => from.QAs, act => act.Ignore())
                ;
            CreateMap<Section, DetailsSectionViewModel>();
            CreateMap<Section, ListSectionsViewModel>();

            // ApplicationUsers
            CreateMap<ApplicationUser, CreateApplicationUserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, DeleteApplicationUserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, ListApplicationUsersViewModel>();
            CreateMap<ApplicationUser, DetailsApplicationUserViewModel>();

            CreateMap<ApplicationUser, EditApplicationUserViewModel>();
            CreateMap<EditApplicationUserViewModel, ApplicationUser>();
        }
    }
}
