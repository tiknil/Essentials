﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;

namespace Xamarin.Essentials
{
    public static partial class MediaPicker
    {
        const int requestCodeMediaPicker = 12348;

        static async Task<MediaFile> PlatformShowPhotoPickerAsync(MediaPickerOptions options)
        {
            // we only need the permission when accessing the file, but it's more natural
            // to ask the user first, then show the picker.
            await Permissions.RequestAsync<Permissions.StorageRead>();

            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("image/*");

            var pickerIntent = Intent.CreateChooser(intent, options?.Title);

            try
            {
                var result = await IntermediateActivity.StartAsync(pickerIntent, requestCodeMediaPicker);

                return new MediaFile(result.Data);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
    }

    public partial class MediaFile
    {
        internal MediaFile(global::Android.Net.Uri contentUri)
            : base(contentUri)
        {
        }
    }
}