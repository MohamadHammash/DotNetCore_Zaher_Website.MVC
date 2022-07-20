namespace Zaher.Ui.HttpClientClasses
{
    public class CodeContainer
    {


        //        var deleteUserPostingCards = (await uoW.PostingCardsRepository.GetAllPostingCardsAsync()).Where(pc => pc.ApplicationUserId == id);
        //        deleteUserPostingCards.Select(async p => {p = await UpdateUserId(p);
        //    });




        //            if (user.VideosLists is not null)
        //            {
        //                user.VideosLists.Select(vl => { vl.ApplicationUserId = superAdminId; return vl; });
        //            }
        //            if (user.Videos is not null)
        //{
        //    user.Videos.Select(v => { v.ApplicationUserId = superAdminId; return v; });
        //}
        //if (user.PostingCards is not null)
        //{
        //    user.PostingCards.Select(pc => { pc.ApplicationUserId = superAdminId; return pc; });
        //}

        //private async Task<T> UpdateUserId<T>(T entity) where T : class
        //{

        //    if (entity != null)
        //    {

        //        if (entity is PostingCard)
        //        {
        //            var postingcard = entity as PostingCard;
        //            var pCardToBeEdited = await uoW.PostingCardsRepository.GetPostingCardByIdAsync(postingcard.Id);

        //            pCardToBeEdited.ApplicationUserId = (await uoW.UsersRepository.GetAllUsersAsync()).Where(u => u.Role.ToLower() == "superadmin").FirstOrDefault().Id;
        //            await TryUpdateModelAsync(pCardToBeEdited, "", p => p.ApplicationUserId);


        //        }
        //        if (entity is VideosList)
        //        {
        //            var videosList = entity as VideosList;
        //            var vLToBeEdited = await uoW.VideosListsRepository.GetVideosListByIdAsync(videosList.Id);

        //            vLToBeEdited.ApplicationUserId = (await uoW.UsersRepository.GetAllUsersAsync()).Where(u => u.Role.ToLower() == "superadmin").FirstOrDefault().Id;
        //            await TryUpdateModelAsync(vLToBeEdited, "", vl => vl.ApplicationUserId);
        //        }
        //        if (entity is Video)
        //        {
        //            var video = entity as Video;
        //            var vToBeEdited = await uoW.VideosRepository.GetVideoByIdAsync(video.Id);

        //            vToBeEdited.ApplicationUserId = (await uoW.UsersRepository.GetAllUsersAsync()).Where(u => u.Role.ToLower() == "superadmin").FirstOrDefault().Id;
        //            await TryUpdateModelAsync(vToBeEdited, "", v => v.ApplicationUserId);
        //        }
        //    }

        //    await uoW.CompleteAsync();
        //    return entity;
        //}
    }
}
