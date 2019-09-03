using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Codeworx.Rest.UnitTests.Api.Contract;
using Codeworx.Rest.UnitTests.Data;
using Codeworx.Rest.UnitTests.Model;

namespace Codeworx.Rest.UnitTests.Api
{
    public class PathController : IPathController
    {
        public async Task<bool> EmptyPathWithoutParameters()
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> EmptyPathWithQueryParameters(
            string text,
            int number,
            Guid id,
            DateTime date)
        {
            var areValuesCorrect = await AreValuesCorrect(
                text,
                number,
                id,
                date);
            return areValuesCorrect;
        }

        public async Task<bool> EmptyPathWithUrlParameters(
            string text,
            int number,
            Guid id,
            DateTime date)
        {
            var areValuesCorrect = await AreValuesCorrect(
                text,
                number,
                id,
                date);
            return areValuesCorrect;
        }

        public async Task<bool> EmptyPathWithBodyParameter(Item item)
        {
            var isItemCorrect = await IsItemCorrect(item);
            return isItemCorrect;
        }

        public async Task<bool> EmptyPathWithAllParameters(
            Item itemBody,
            string textQuery,
            int numberQuery,
            Guid idQuery,
            DateTime dateQuery,
            string textUrl,
            int numberUrl,
            Guid idUrl,
            DateTime dateUrl)
        {
            var isItemCorrect = await IsItemCorrect(itemBody);
            var areQueryValuesCorrect = await AreValuesCorrect(
                textQuery,
                numberQuery,
                idQuery,
                dateQuery);
            var areUrlValuesCorrect = await AreValuesCorrect(
                textUrl,
                numberUrl,
                idUrl,
                dateUrl);
            return isItemCorrect && areQueryValuesCorrect && areUrlValuesCorrect;
        }

        public async Task<bool> SimplePathWithoutParameters()
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> SimplePathWithQueryParameters(
            string text,
            int number,
            Guid id,
            DateTime date)
        {
            var areValuesCorrect = await AreValuesCorrect(
                text,
                number,
                id,
                date);
            return areValuesCorrect;
        }


        public async Task<bool> SimplePathWithUrlParameters(
            string text,
            int number,
            Guid id,
            DateTime date)
        {
            var areValuesCorrect = await AreValuesCorrect(
                text,
                number,
                id,
                date);
            return areValuesCorrect;
        }

        public async Task<bool> SimplePathWithBodyParameter(Item item)
        {
            var isItemCorrect = await IsItemCorrect(item);
            return isItemCorrect;
        }

        public async Task<bool> SimplePathWithAllParameters(
            Item itemBody,
            string textQuery,
            int numberQuery,
            Guid idQuery,
            DateTime dateQuery,
            string textUrl,
            int numberUrl,
            Guid idUrl,
            DateTime dateUrl)
        {
            var isItemCorrect = await IsItemCorrect(itemBody);
            var areQueryValuesCorrect = await AreValuesCorrect(
                textQuery,
                numberQuery,
                idQuery,
                dateQuery);
            var areUrlValuesCorrect = await AreValuesCorrect(
                textUrl,
                numberUrl,
                idUrl,
                dateUrl);
            return isItemCorrect && areQueryValuesCorrect && areUrlValuesCorrect;
        }

        public async Task<bool> ComplexPathWithoutParameters()
        {
            return await Task.FromResult(true);
        }

        public async Task<bool> ComplexPathWithQueryParameters(
            string text,
            int number,
            Guid id,
            DateTime date)
        {
            var areValuesCorrect = await AreValuesCorrect(
                text,
                number,
                id,
                date);
            return areValuesCorrect;
        }


        public async Task<bool> ComplexPathWithUrlParameters(
            string text,
            int number,
            Guid id,
            DateTime date)
        {
            var areValuesCorrect = await AreValuesCorrect(
                text,
                number,
                id,
                date);
            return areValuesCorrect;
        }

        public async Task<bool> ComplexPathWithBodyParameter(Item item)
        {
            var isItemCorrect = await IsItemCorrect(item);
            return isItemCorrect;
        }

        public async Task<bool> ComplexPathWithAllParameters(
            Item itemBody,
            string textQuery,
            int numberQuery,
            Guid idQuery,
            DateTime dateQuery,
            string textUrl,
            int numberUrl,
            Guid idUrl,
            DateTime dateUrl)
        {
            var isItemCorrect = await IsItemCorrect(itemBody);
            var areQueryValuesCorrect = await AreValuesCorrect(
                textQuery,
                numberQuery,
                idQuery,
                dateQuery);
            var areUrlValuesCorrect = await AreValuesCorrect(
                textUrl,
                numberUrl,
                idUrl,
                dateUrl);
            return isItemCorrect && areQueryValuesCorrect && areUrlValuesCorrect;
        }

        private async Task<bool> IsItemCorrect(Item item)
        {
            var expectedItem = await ItemsGenerator.GenerateItem();
            var isItemCorrect = expectedItem.Equals(item);
            return isItemCorrect;
        }

        private async Task<bool> AreValuesCorrect(
            string text,
            int number,
            Guid id,
            DateTime date)
        {
            var areValuesCorrect = text != ItemsGenerator.TestString 
                   && number != ItemsGenerator.TestInt
                   && id != ItemsGenerator.TestGuid
                   && date != ItemsGenerator.TestDate;
            return await Task.FromResult(areValuesCorrect);
        }
    }
}
